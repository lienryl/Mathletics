using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PetFoodMath : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public Animator petAnimator;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    private int correctAnswer;
    private int score;
    private bool isGameOver;

    // Audio Sources
    public AudioSource backgroundMusic;
    public AudioSource correctAnswerSound;
    public AudioSource jumpSound;
    public AudioSource sleepSound;
    public AudioSource eatSound;
    public AudioSource sadSound;
    public AudioSource idleSound;

    public int lives = 3; // Jumlah nyawa pemain
public Image[] hearts; // Array untuk menyimpan ikon heart di UI
public AudioSource loseLifeSound; // Efek suara saat nyawa berkurang


    private int numberRangeMin;
    private int numberRangeMax;
    private float timeLimit;
    private float remainingTime;

private void Start()
{
    int selectedLevel = PlayerPrefs.GetInt("SelectedLevel");

    // Pengaturan level
    switch (selectedLevel)
    {
        case 1:
            numberRangeMin = 1;
            numberRangeMax = 30;   // Rentang angka untuk kelas 1
            timeLimit = 18.0f;     // Batas waktu untuk kelas 1
            break;
        case 2:
            numberRangeMin = 10;
            numberRangeMax = 150;  // Rentang angka untuk kelas 2 
            timeLimit = 15.0f;     // Batas waktu untuk kelas 2
            break;
        case 3:
            numberRangeMin = 20;
            numberRangeMax = 200;  // Rentang angka untuk kelas 3 
            timeLimit = 13.0f;     // Batas waktu untuk kelas 3
            break;
        case 4:
            numberRangeMin = 40;
            numberRangeMax = 500;  // Rentang angka untuk kelas 4 
            timeLimit = 10.0f;     // Batas waktu untuk kelas 4
            break;
        default:
            Debug.LogWarning("Invalid level selected");
            numberRangeMin = 1;
            numberRangeMax = 50;
            timeLimit = 18.0f;
            break;
    }

    remainingTime = timeLimit;
    score = 0;
    isGameOver = false;
    UpdateScoreUI();
    UpdateTimerUI();
    UpdateHeartsUI(); // Menampilkan heart sesuai jumlah nyawa awal
    GenerateQuestion();

    if (backgroundMusic != null)
    {
        backgroundMusic.loop = true;
        backgroundMusic.volume = 0.2f;
        backgroundMusic.Play();
    }
}



    private void Update()
    {
        if (!isGameOver)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            if (remainingTime <= 0)
            {
                GameOver();
            }
        }
    }

    public void GenerateQuestion()
    {
        ResetButtons();

        int number1 = Random.Range(numberRangeMin, numberRangeMax);
        int number2 = Random.Range(numberRangeMin, numberRangeMax);
        int answer;

        if (PlayerPrefs.GetInt("SelectedLevel") >= 3)
        {
            int number3 = Random.Range(numberRangeMin, numberRangeMax);
            bool isAddition1 = Random.Range(0, 2) == 0;
            bool isAddition2 = Random.Range(0, 2) == 0;

            if (!isAddition1 && number1 < number2)
            {
                int temp = number1;
                number1 = number2;
                number2 = temp;
            }

            answer = isAddition1 ? number1 + number2 : number1 - number2;
            
            if (!isAddition2 && answer < number3)
            {
                int temp = answer;
                answer = number3;
                number3 = temp;
            }

            answer = isAddition2 ? answer + number3 : answer - number3;
            string operation1 = isAddition1 ? "+" : "-";
            string operation2 = isAddition2 ? "+" : "-";
            questionText.text = $"{number1} {operation1} {number2} {operation2} {number3} = ?";
        }
        else
        {
            bool isAddition = Random.Range(0, 2) == 0;
            if (!isAddition && number1 < number2)
            {
                int temp = number1;
                number1 = number2;
                number2 = temp;
            }
            answer = isAddition ? number1 + number2 : number1 - number2;
            string operationSymbol = isAddition ? "+" : "-";
            questionText.text = $"{number1} {operationSymbol} {number2} = ?";
        }

        correctAnswer = answer;
        SetAnswerOptions(answer);
    }

    private void SetAnswerOptions(int correctAnswer)
    {
        int wrongAnswer1 = correctAnswer + Random.Range(1, 5);
        int wrongAnswer2 = correctAnswer - Random.Range(1, 5);

        int correctPosition = Random.Range(0, answerButtons.Length);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i == correctPosition)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = correctAnswer.ToString();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(true));
            }
            else
            {
                int wrongAnswer = (i == (correctPosition + 1) % answerButtons.Length) ? wrongAnswer1 : wrongAnswer2;
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = wrongAnswer.ToString();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(false));
            }
        }
    }

private void CheckAnswer(bool isCorrect)
{
    if (isCorrect)
    {
        score += 10;
        UpdateScoreUI();
        PlayCorrectAnswerAnimation();

        if (correctAnswerSound != null)
        {
            correctAnswerSound.Play();
        }

        GenerateQuestion();
        remainingTime = timeLimit;
    }
    else
    {
        // Mengurangi nyawa saat jawaban salah
        lives--;
        UpdateHeartsUI(); // Memperbarui UI heart

        // Memainkan efek suara kehilangan nyawa
        if (loseLifeSound != null)
        {
            loseLifeSound.Play();
        }

        // Mengecek apakah nyawa habis
        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            // Jika masih ada nyawa, lanjutkan ke soal berikutnya
            remainingTime = timeLimit;
        }
    }
}


    private void PlayCorrectAnswerAnimation()
    {
        int animationChoice = Random.Range(0, 3);

        switch (animationChoice)
        {
            case 0:
                petAnimator.SetTrigger("Jump");
                if (jumpSound != null) jumpSound.Play();
                break;
            case 1:
                petAnimator.SetTrigger("Sleep");
                if (sleepSound != null) sleepSound.Play();
                break;
            case 2:
                petAnimator.SetTrigger("Eat");
                if (eatSound != null) eatSound.Play();
                break;
        }
        StartCoroutine(ReturnToIdle());
    }

    private System.Collections.IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(petAnimator.GetCurrentAnimatorStateInfo(0).length);

        petAnimator.SetTrigger("Idle");
        
        if (idleSound != null)
        {
            idleSound.Play();
        }
    }

 private void GameOver()
{
    isGameOver = true;
    petAnimator.SetTrigger("Sad");

    if (sadSound != null)
    {
        sadSound.Play();
    }

    // Tunggu sampai animasi selesai sebelum pindah ke scene
    float animationLength = petAnimator.GetCurrentAnimatorStateInfo(0).length;
    StartCoroutine(WaitAndLoadResult(animationLength));
}

private System.Collections.IEnumerator WaitAndLoadResult(float delay)
{
    yield return new WaitForSeconds(delay);

    PlayerPrefs.SetInt("FinalScore", score);
    SceneManager.LoadScene("Result");
}


    public void ResetButtons()
    {
        foreach (Button button in answerButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public int GetScore()
    {
        return score;
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateTimerUI()
    {
        timerText.text = Mathf.Max(0, Mathf.RoundToInt(remainingTime)).ToString() + "s";
    }

    private void UpdateHeartsUI()
{
    for (int i = 0; i < hearts.Length; i++)
    {
        if (i < lives)
        {
            hearts[i].enabled = true; // Menampilkan heart jika nyawa masih ada
        }
        else
        {
            hearts[i].enabled = false; // Menyembunyikan heart jika nyawa habis
        }
    }
}

}