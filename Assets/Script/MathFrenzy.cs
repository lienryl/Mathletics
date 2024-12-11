using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MathFrenzy : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText; // Elemen UI untuk menampilkan skor

    public Button trueButton;
    public Button falseButton;
    public Slider timeSlider;

    // Variabel Audio
    public AudioSource backgroundMusic;
    public AudioSource correctAnswerSound;

    private float currentTime;
    private float totalTime;
    private int score;
    private bool isGameOver;
    private bool currentAnswerIsTrue;
    private bool isTimerRunning;
    private float timeLimit;
    private int numberRange;
    private int multiplicationDivisionRange;

    void Start()
    {
        int selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 1); // Default level 1 jika belum diatur

        // Mengatur batas angka dan waktu sesuai level
        SetNumberRange(selectedLevel);

        score = 0;
        UpdateScoreUI();
        totalTime = 0;
        isGameOver = false;
        isTimerRunning = false;

        GenerateQuestion();

        // Menambahkan listener pada tombol benar dan salah
        trueButton.onClick.AddListener(() => CheckAnswer(true));
        falseButton.onClick.AddListener(() => CheckAnswer(false));

        ResetTimer();

        // Memutar musik latar jika diatur
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    void Update()
    {
        if (isGameOver || !isTimerRunning)
            return;

        // Mengurangi waktu berjalan dan memperbarui slider
        currentTime -= Time.deltaTime;
        totalTime += Time.deltaTime;
        timeSlider.value = currentTime / timeLimit;

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    // Mengatur batas angka dan waktu sesuai level
private void SetNumberRange(int level)
{
    switch (level)
    {
        case 1:
            numberRange = 50;                 // Rentang untuk operasi tambah dan kurang
            multiplicationDivisionRange = 10; // Rentang untuk perkalian dan pembagian
            timeLimit = 18.0f;
            break;
        case 2:
            numberRange = 150;
            multiplicationDivisionRange = 20;
            timeLimit = 15.0f;
            break;
        case 3:
            numberRange = 300;
            multiplicationDivisionRange = 30;
            timeLimit = 13.0f;
            break;
        case 4:
            numberRange = 500;
            multiplicationDivisionRange = 50;
            timeLimit = 10.0f;
            break;
        default:
            numberRange = 50;
            multiplicationDivisionRange = 10;
            timeLimit = 18.0f;
            break;
    }
}


    // Fungsi untuk menghasilkan soal baru
   private void GenerateQuestion()
{
    ResetTimer();

    int number1, number2, result;
    string operationSymbol;
    bool isCorrectResult;

    int selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 1);
    int operation = Random.Range(0, 2); // Hanya 0 dan 1 untuk penjumlahan dan pengurangan

    bool generateTrueAnswer = Random.Range(0, 2) == 0;

    // Batasi jenis soal untuk level 1 dan 2
    if (selectedLevel <= 2)
    {
        switch (operation)
        {
            case 0: // Penjumlahan
                operationSymbol = "+";
                number1 = Random.Range(1, numberRange + 1);
                number2 = Random.Range(1, numberRange + 1);

                result = generateTrueAnswer ? number1 + number2 : Random.Range(number1 + number2 - 5, number1 + number2 + 5);
                isCorrectResult = (result == number1 + number2);
                questionText.text = $"{number1} + {number2} = {result}";
                break;

            case 1: // Pengurangan
                operationSymbol = "-";
                number1 = Random.Range(1, numberRange + 1);
                number2 = Random.Range(1, numberRange + 1);

                if (number1 < number2) // Pastikan hasil tidak negatif
                {
                    int temp = number1;
                    number1 = number2;
                    number2 = temp;
                }

                result = generateTrueAnswer ? number1 - number2 : Random.Range(number1 - number2 - 5, number1 - number2 + 5);
                isCorrectResult = (result == number1 - number2);
                questionText.text = $"{number1} - {number2} = {result}";
                break;

            default:
                // Fallback jika terjadi error
                operationSymbol = "+";
                number1 = 1;
                number2 = 1;
                result = 2;
                isCorrectResult = true;
                questionText.text = $"{number1} + {number2} = {result}";
                break;
        }
    }
    else
    {
        // Soal untuk level di atas 2 (termasuk perkalian dan pembagian)
        int operationFull = Random.Range(0, 4);
        switch (operationFull)
        {
            case 0: // Penjumlahan
                operationSymbol = "+";
                number1 = Random.Range(1, numberRange + 1);
                number2 = Random.Range(1, numberRange + 1);
                result = generateTrueAnswer ? number1 + number2 : Random.Range(number1 + number2 - 10, number1 + number2 + 10);
                isCorrectResult = (result == number1 + number2);
                questionText.text = $"{number1} + {number2} = {result}";
                break;

            case 1: // Pengurangan
                operationSymbol = "-";
                number1 = Random.Range(1, numberRange + 1);
                number2 = Random.Range(1, numberRange + 1);
                if (number1 < number2)
                {
                    int temp = number1;
                    number1 = number2;
                    number2 = temp;
                }
                result = generateTrueAnswer ? number1 - number2 : Random.Range(number1 - number2 - 10, number1 - number2 + 10);
                isCorrectResult = (result == number1 - number2);
                questionText.text = $"{number1} - {number2} = {result}";
                break;

            case 2: // Perkalian
                operationSymbol = "×";
                number1 = Random.Range(1, multiplicationDivisionRange + 1);
                number2 = Random.Range(1, multiplicationDivisionRange + 1);
                result = generateTrueAnswer ? number1 * number2 : Random.Range(number1 * number2 - 10, number1 * number2 + 10);
                isCorrectResult = (result == number1 * number2);
                questionText.text = $"{number1} × {number2} = {result}";
                break;

            case 3: // Pembagian
                operationSymbol = "÷";
                number2 = Random.Range(1, multiplicationDivisionRange + 1);
                number1 = number2 * Random.Range(1, multiplicationDivisionRange + 1);
                result = generateTrueAnswer ? number1 / number2 : Random.Range(number1 / number2 - 3, number1 / number2 + 3);
                isCorrectResult = (result == number1 / number2);
                questionText.text = $"{number1} ÷ {number2} = {result}";
                break;

            default:
                operationSymbol = "+";
                number1 = 1;
                number2 = 1;
                result = 2;
                isCorrectResult = true;
                questionText.text = $"{number1} + {number2} = {result}";
                break;
        }
    }

    currentAnswerIsTrue = isCorrectResult;
}


    // Fungsi untuk memeriksa jawaban
    private void CheckAnswer(bool answer)
    {
        if (answer == currentAnswerIsTrue)
        {
            score += 10;
             UpdateScoreUI(); 
            if (!isTimerRunning)
            {
                isTimerRunning = true;
            }

            // Memainkan suara jika jawaban benar
            if (correctAnswerSound != null)
            {
                correctAnswerSound.Play();
            }

            GenerateQuestion();
        }
        else
        {
            GameOver();
        }
    }

    // Mengatur ulang timer setiap kali soal baru dihasilkan
    private void ResetTimer()
    {
        currentTime = timeLimit;
        timeSlider.value = 1;
    }

    // Fungsi ketika permainan berakhir
    private void GameOver()
    {
        isGameOver = true;
        isTimerRunning = false;
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetFloat("TotalTime", totalTime);
        SceneManager.LoadScene("ResultFrenzy");
    }

    // Mengambil skor saat ini
    public int GetScore()
    {
        return score;
    }

    private void UpdateScoreUI()
{
    scoreText.text = "Score: " + score; // Mengupdate tampilan skor
}

}