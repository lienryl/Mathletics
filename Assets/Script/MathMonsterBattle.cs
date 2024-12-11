using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathMonsterBattle : MonoBehaviour
{
    // Elemen UI
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public Animator playerAnimator;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 10f;

    private int correctAnswer;
    private int score;
    public TextMeshProUGUI scoreText;
    private bool isGameOver;

    // UI untuk nyawa
    public Image[] heartIcons; // Array ikon nyawa, hubungkan di Inspector
    private int lives = 3; // Pemain memiliki 3 kesempatan

    // Elemen monster dan suara
    public GameObject monsterPrefab; 
    public Transform monsterSpawnPoint;
    private GameObject currentMonster;
    private float timeLimit;
    private int numberRange;

    public AudioSource hitSound;
    public AudioSource loseLifeSound; // Tambahkan referensi untuk suara kehilangan nyawa
    public AudioSource backgroundMusic;

    public float musicVolume = 0.05f;
    public float soundEffectVolume = 0.5f;

private void Start()
{
    // Mengatur volume audio dan memutar musik latar
    backgroundMusic.volume = musicVolume;
    hitSound.volume = soundEffectVolume;
    backgroundMusic.loop = true;
    backgroundMusic.Play();

    // Mengatur kesulitan berdasarkan level (rentang angka dan batas waktu)
    int selectedLevel = PlayerPrefs.GetInt("SelectedLevel");

    switch (selectedLevel)
    {
        case 1:
            numberRange = 30;    // Rentang angka untuk kelas 1 (1-50)
            timeLimit = 18.0f;   // Batas waktu untuk kelas 1
            break;
        case 2:
            numberRange = 150;   // Rentang angka untuk kelas 2 (1-150)
            timeLimit = 15.0f;   // Batas waktu untuk kelas 2
            break;
        case 3:
            numberRange = 300;   // Rentang angka untuk kelas 3 (1-300)
            timeLimit = 13.0f;   // Batas waktu untuk kelas 3
            break;
        case 4:
            numberRange = 500;   // Rentang angka untuk kelas 4 (1-500)
            timeLimit = 10.0f;   // Batas waktu untuk kelas 4
            break;
        default:
            Debug.LogWarning("Level yang dipilih tidak valid");
            numberRange = 50;    // Default ke kelas 1 jika level tidak valid
            timeLimit = 18.0f;
            break;
    }

    score = 0;
    UpdateScoreUI(); // Menampilkan skor awal
    UpdateLivesUI(); // Menampilkan jumlah nyawa awal
    GenerateQuestion(); // Menghasilkan pertanyaan pertama
    SpawnMonster(); // Memunculkan monster pertama
}


    public void GenerateQuestion()
    {
        ResetButtons(); // Menghapus listener yang ada pada tombol jawaban
    int selectedLevel = PlayerPrefs.GetInt("SelectedLevel");
    int number1 = Random.Range(1, numberRange);
    int number2 = Random.Range(1, numberRange);
    int answer;
    string question;

    // Menghasilkan soal berdasarkan level
    if (selectedLevel >= 3)
    {
        // Operasi dengan tiga angka untuk level lebih tinggi
        int number3 = Random.Range(1, numberRange);
        bool isAddition = Random.Range(0, 2) == 0;
        bool isSubtraction = Random.Range(0, 2) == 0;

        if (isAddition)
        {
            if (isSubtraction)
            {
                answer = number1 - number2 + number3;
                question = $"{number1} - {number2} + {number3} = ?";
            }
            else
            {
                answer = number1 + number2 - number3;
                question = $"{number1} + {number2} - {number3} = ?";
            }
        }
        else
        {
            answer = number1 + number2 + number3;
            question = $"{number1} + {number2} + {number3} = ?";
        }
    }
    else
    {
        // Operasi dengan dua angka untuk level lebih rendah
        bool isAddition = Random.Range(0, 2) == 0;

        if (isAddition)
        {
            answer = number1 + number2;
            question = $"{number1} + {number2} = ?";
        }
        else
        {
            // Memastikan hasil positif untuk pengurangan
            if (number1 < number2)
            {
                int temp = number1;
                number1 = number2;
                number2 = temp;
            }
            answer = number1 - number2;
            question = $"{number1} - {number2} = ?";
        }
    }

    correctAnswer = answer;
    questionText.text = question; // Menampilkan pertanyaan yang dihasilkan
    SetAnswerOptions(correctAnswer); // Menyusun opsi jawaban
    }

    private void SetAnswerOptions(int correctAnswer)
    {
          // Menghasilkan jawaban salah mendekati jawaban benar
    int wrongAnswer1 = correctAnswer + Random.Range(5, 15);
    int wrongAnswer2 = correctAnswer - Random.Range(5, 15);

    // Pastikan jawaban salah tidak negatif
    if (wrongAnswer2 < 0)
    {
        wrongAnswer2 = correctAnswer + Random.Range(5, 15);
    }

    // Menentukan posisi jawaban benar secara acak di antara tombol
    int correctPosition = Random.Range(0, answerButtons.Length);

    for (int i = 0; i < answerButtons.Length; i++)
    {
        if (i == correctPosition)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = correctAnswer.ToString();
            answerButtons[i].onClick.AddListener(() => {
                CheckAnswer(true);
            });
        }
        else
        {
            // Menetapkan jawaban salah ke tombol
            int wrongAnswer = (i == (correctPosition + 1) % answerButtons.Length) ? wrongAnswer1 : wrongAnswer2;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = wrongAnswer.ToString();
            answerButtons[i].onClick.AddListener(() => {
                CheckAnswer(false);
            });
        }
    }
    }

public void CheckAnswer(bool isCorrect)
{
    if (isCorrect)
    {
        ShootArrow(); // Memicu animasi dan aksi menyerang
        StartCoroutine(ReturnToIdle());
    }
    else
    {
        // Mengurangi nyawa pemain
        lives--;
        UpdateLivesUI(); // Perbarui UI dan periksa kondisi nyawa habis

        // Memainkan suara nyawa berkurang
        if (loseLifeSound != null)
        {
            loseLifeSound.Play();
        }
    }
}

private void UpdateLivesUI()
{
    // Perbarui tampilan nyawa di UI
    for (int i = 0; i < heartIcons.Length; i++)
    {
        heartIcons[i].enabled = i < lives; // Menonaktifkan ikon jika nyawa habis
    }

    // Periksa jika nyawa habis
    if (lives <= 0)
    {
        GameOver(); // Panggil GameOver jika nyawa 0
    }
}


    private System.Collections.IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        playerAnimator.SetTrigger("Idle"); // Kembali ke keadaan idle setelah animasi menembak selesai
    }

    private void ShootArrow()
    {
        playerAnimator.SetTrigger("Shoot"); // Memainkan animasi menembak
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = Vector2.right * arrowSpeed; // Mengatur kecepatan gerakan panah
    }

 public void ArrowHitMonster()
{
    if (currentMonster != null)
    {
        Monster monsterScript = currentMonster.GetComponent<Monster>();
        if (monsterScript != null)
        {
            monsterScript.Die(); // Memicu animasi mati pada monster
        }

        hitSound.Play(); // Memainkan efek suara saat mengenai

        // Tambahkan skor dan perbarui UI
        score += 10;
        UpdateScoreUI();
    }

    // Memunculkan monster baru setelah monster lama terkena
    SpawnMonster();
    GenerateQuestion();
}


    private void SpawnMonster()
    {
        currentMonster = Instantiate(monsterPrefab, monsterSpawnPoint.position, Quaternion.identity);

        // Mengatur kecepatan monster berdasarkan batas waktu
        Monster monsterScript = currentMonster.GetComponent<Monster>();
        if (monsterScript != null)
        {
            monsterScript.SetSpeed(CalculateMonsterSpeed());
        }
    }

    private float CalculateMonsterSpeed()
    {
        float minSpeed = 1.0f;
        float maxSpeed = 5.0f;
        return Mathf.Lerp(maxSpeed, minSpeed, timeLimit / 10.0f); // Mengatur kecepatan sesuai level
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Animasi mati pemain
        playerAnimator.SetTrigger("Die");

        // Tunggu sebelum pindah ke hasil
        StartCoroutine(WaitAndLoadResultScene());
    }

private System.Collections.IEnumerator WaitAndLoadResultScene()
{
    // Ambil informasi animasi yang sedang dimainkan
    AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

    // Tunggu sampai animasi selesai
    yield return new WaitForSeconds(stateInfo.length);

    // Lanjutkan ke layar hasil
    PlayerPrefs.SetInt("FinalScore", score);
    UnityEngine.SceneManagement.SceneManager.LoadScene("Result");
}

    public void ResetButtons()
    {
        foreach (Button button in answerButtons)
        {
            button.onClick.RemoveAllListeners(); // Menghapus listener pada tombol
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score; // Memperbarui tampilan skor
    }

    public float GetTimeLimit()
    {
        return timeLimit; // Getter untuk batas waktu
    }

}