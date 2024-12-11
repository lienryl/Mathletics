using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TableMath : MonoBehaviour
{
    // Referensi untuk teks tampilan soal
    public TextMeshProUGUI number1Text;
    public TextMeshProUGUI number2Text;
    public TextMeshProUGUI operation;
    public TextMeshProUGUI equal;
    public TextMeshProUGUI answer;
    public TextMeshProUGUI scoreText; // Elemen UI untuk menampilkan skor


    // Tombol pilihan jawaban
    public Button[] answerButtons;
    public Slider timeSlider; // Slider untuk menunjukkan waktu yang tersisa

    private int correctAnswer; // Jawaban benar
    private int score; // Skor pemain
    private int numberRange; // Rentang angka berdasarkan level
    private float remainingTime; // Waktu yang tersisa untuk menjawab

    private float timeLimit; // Batas waktu per soal sesuai level

    // Variabel Audio
    public AudioSource backgroundMusic;
    public AudioSource correctAnswerSound;

private void Start()
{
    // Ambil level yang dipilih dari PlayerPrefs
    int selectedLevel = PlayerPrefs.GetInt("SelectedLevel");

    // Atur rentang angka dan batas waktu berdasarkan level yang dipilih
    switch (selectedLevel)
    {
        case 1: 
            numberRange = 10;  // Level 1: Rentang angka 1 - 15
            timeLimit = 18.0f; // Batas waktu: 18 detik
            break;
        case 2: 
            numberRange = 20;  // Level 2: Rentang angka 1 - 30
            timeLimit = 15.0f; // Batas waktu: 15 detik
            break;
        case 3: 
            numberRange = 30;  // Level 3: Rentang angka 1 - 50
            timeLimit = 13.0f; // Batas waktu: 13 detik
            break;
        case 4: 
            numberRange = 50; // Level 4: Rentang angka 1 - 100
            timeLimit = 10.0f; // Batas waktu: 10 detik
            break;
        default:
            Debug.LogWarning("Level tidak valid, menggunakan nilai default.");
            numberRange = 15;
            timeLimit = 10f;
            break;
    }

    score = 0; // Inisialisasi skor
    UpdateScoreUI(); // Perbarui tampilan skor di awal
    GenerateQuestion(); // Buat soal pertama
    ResetTimer(); // Atur ulang timer

    // Memainkan musik latar belakang jika tersedia
    if (backgroundMusic != null)
    {
        backgroundMusic.loop = true; // Musik diputar berulang
        backgroundMusic.Play();
    }
}


    private void Update()
    {
        // Perbarui slider waktu dan periksa jika waktu habis
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timeSlider.value = remainingTime / timeLimit; // Update slider waktu

            if (remainingTime <= 0)
            {
                GameOver(); // Jika waktu habis, permainan berakhir
            }
        }
    }

    private void GenerateQuestion()
    {
        ResetButtons(); // Reset tombol jawaban
        ResetTimer(); // Reset waktu untuk soal baru

        // Buat angka acak untuk soal
        int number1 = Random.Range(1, numberRange); 
        int number2 = Random.Range(1, numberRange);
        string timesOperation = "X"; // Simbol operasi kali
        string equalString = "=";
        correctAnswer = number1 * number2; // Jawaban benar adalah hasil perkalian

        // Pilih secara acak apakah jawaban atau angka kedua yang disembunyikan
        bool hideAnswer = Random.value > 0.5f;

        if (hideAnswer)
        {
            // Tampilkan soal dengan jawaban disembunyikan
            number1Text.text = $"{number1}";
            operation.text = $"{timesOperation}";
            number2Text.text = $"{number2}";
            equal.text = $"{equalString}";
            answer.text = "?";
            SetAnswerOptions(correctAnswer); // Set pilihan jawaban
        }
        else
        {
            // Tampilkan soal dengan angka kedua disembunyikan
            number1Text.text = $"{number1}";
            operation.text = $"{timesOperation}";
            number2Text.text = "?";
            equal.text = $"{equalString}";
            answer.text = $"{correctAnswer}";
            SetAnswerOptions(number2); // Jawaban yang benar adalah angka kedua
        }
    }

    private void SetAnswerOptions(int correctAnswer)
    {
        // Buat tiga jawaban salah
        int wrongAnswer1 = correctAnswer + Random.Range(1, 10);
        int wrongAnswer2 = correctAnswer - Random.Range(1, 10);
        if (wrongAnswer2 < 1) wrongAnswer2 = correctAnswer + Random.Range(11, 20); // Hindari jawaban negatif
        int wrongAnswer3 = correctAnswer + Random.Range(11, 20);

        // Pilih posisi acak untuk jawaban benar di antara tombol-tombol
        int correctPosition = Random.Range(0, answerButtons.Length);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i == correctPosition)
            {
                // Jika posisi benar, set jawaban benar di sini
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = correctAnswer.ToString();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(true)); // Tambahkan event listener untuk jawaban benar
            }
            else
            {
                // Jika posisi salah, set jawaban salah
                int wrongAnswer = (i == (correctPosition + 1) % answerButtons.Length) ? wrongAnswer1 :
                                  (i == (correctPosition + 2) % answerButtons.Length) ? wrongAnswer2 : wrongAnswer3;

                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = wrongAnswer.ToString();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(false)); // Tambahkan event listener untuk jawaban salah
            }
        }
    }

    private void CheckAnswer(bool isCorrect)
    {
        if (isCorrect)
        {
            score += 10; // Tambah skor jika benar

            // Mainkan efek suara jika jawaban benar
            if (correctAnswerSound != null)
            {
                correctAnswerSound.Play();
            }
            UpdateScoreUI(); // Perbarui tampilan skor
            GenerateQuestion(); // Buat soal baru
        }
        else
        {
            GameOver(); // Jika jawaban salah, permainan berakhir
        }
    }

    private void ResetButtons()
    {
        // Hapus semua listener pada tombol untuk memastikan tidak ada listener berulang
        foreach (Button button in answerButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void ResetTimer()
    {
        // Atur ulang timer untuk soal baru
        remainingTime = timeLimit;
        timeSlider.value = 1; // Set slider penuh
    }

    private void GameOver()
    {
        // Simpan skor akhir dan pindah ke layar hasil
        PlayerPrefs.SetInt("FinalScore", score);
        SceneManager.LoadScene("Result");
    }

    public int GetScore()
    {
        return score; // Mengembalikan skor untuk penggunaan eksternal jika diperlukan
    }

    private void UpdateScoreUI()
{
    scoreText.text = "Score: " + score; // Tampilkan skor di UI
}

}