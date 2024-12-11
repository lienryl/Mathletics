using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FillTheGap : MonoBehaviour
{
    public TextMeshProUGUI questionText;       // Teks untuk menampilkan pertanyaan
     public TextMeshProUGUI scoreText; // Elemen UI untuk menampilkan skor
    public Button[] operationButtons;          // Tombol untuk memilih operasi matematika
    public Slider timeSlider;                  // Slider untuk menampilkan waktu yang tersisa
    public AudioSource backgroundMusic;        // Musik latar
    public AudioSource correctAnswerSound;     // Suara saat jawaban benar

    private int score;
    private float currentTime;
    private bool isGameOver;
    private string correctOperation;
    private float timeLimit;
    private int selectedLevel;

    void Start()
    {
        selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 1);
        SetNumberRange(selectedLevel);

        score = 0;
        UpdateScoreUI();
        isGameOver = false;
        SetupButtons();
        PlayBackgroundMusic();
        GenerateQuestion();
        ResetTimer();
    }

    void Update()
    {
        if (isGameOver)
            return;

        currentTime -= Time.deltaTime;
        timeSlider.value = currentTime / timeLimit;

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    private void SetupButtons()
    {
        operationButtons[0].onClick.AddListener(() => CheckAnswer("+"));
        operationButtons[1].onClick.AddListener(() => CheckAnswer("-"));
        operationButtons[2].onClick.AddListener(() => CheckAnswer("X"));
        operationButtons[3].onClick.AddListener(() => CheckAnswer("÷"));
    }

private void SetNumberRange(int level)
{
    switch (level)
    {
        case 1:
            timeLimit = 18.0f;
            break;
        case 2:
            timeLimit = 15.0f;
            break;
        case 3:
            timeLimit = 13.0f;
            break;
        case 4:
            timeLimit = 10.0f;
            break;
        default:
            timeLimit = 10.0f;
            break;
    }
}

private void GenerateQuestion()
{
    ResetTimer();

    switch (selectedLevel)
    {
        case 1:
            GenerateTwoNumberQuestion(50, 10);   // Level 1
            break;
        case 2:
            GenerateTwoNumberQuestion(150, 20);  // Level 2
            break;
        case 3:
            GenerateThreeNumberQuestion(300, 30); // Level 3
            break;
        case 4:
            GenerateThreeNumberQuestion(500, 50); // Level 4
            break;
    }
}

private void GenerateTwoNumberQuestion(int addSubRange, int multiDivRange)
{
    int number1 = Random.Range(1, addSubRange);
    int number2 = Random.Range(1, addSubRange);

    int operationChoice;
    
    // Batasi operasi untuk level 1 dan 2
    if (selectedLevel <= 2)
    {
        operationChoice = Random.Range(0, 2); // Hanya penjumlahan dan pengurangan
    }
    else
    {
        operationChoice = Random.Range(0, 4); // Semua operasi
    }

    int result = 0;

    switch (operationChoice)
    {
        case 0: // Penjumlahan
            correctOperation = "+";
            result = number1 + number2;
            break;

        case 1: // Pengurangan
            correctOperation = "-";
            if (number1 < number2) 
            {
                int temp = number1;
                number1 = number2;
                number2 = temp;
            }
            result = number1 - number2;
            break;

        case 2: // Perkalian
            correctOperation = "X";
            number1 = Random.Range(2, multiDivRange); 
            number2 = Random.Range(2, multiDivRange);
            result = number1 * number2;
            break;

        case 3: // Pembagian
            correctOperation = "÷";
            number1 = Random.Range(2, multiDivRange) * Random.Range(2, multiDivRange);
            number2 = Random.Range(2, multiDivRange);
            result = number1 / number2;
            break;
    }

    questionText.text = $"{number1} ? {number2} = {result}";
}


  private void GenerateThreeNumberQuestion(int addSubRange, int multiDivRange)
{
    if (selectedLevel <= 2)
    {
        // Jika level 1 atau 2, tetap gunakan GenerateTwoNumberQuestion
        GenerateTwoNumberQuestion(addSubRange, multiDivRange);
        return;
    }

    int number1 = Random.Range(1, addSubRange);
    int number2 = Random.Range(1, addSubRange);
    int number3 = Random.Range(1, addSubRange);

    int result = 0;
    string operation1 = "";
    string operation2 = "";

    int operationChoice1 = Random.Range(0, 4);
    int operationChoice2 = Random.Range(0, 4);

    switch (operationChoice1)
    {
        case 0: operation1 = "+"; result = number1 + number2; break;
        case 1: operation1 = "-"; if (number1 < number2) { int temp = number1; number1 = number2; number2 = temp; } result = number1 - number2; break;
        case 2: operation1 = "X"; number1 = Random.Range(2, multiDivRange); number2 = Random.Range(2, multiDivRange); result = number1 * number2; break;
        case 3: operation1 = "÷"; number1 = Random.Range(2, multiDivRange) * Random.Range(2, multiDivRange); number2 = Random.Range(2, multiDivRange); result = number1 / number2; break;
    }

    switch (operationChoice2)
    {
        case 0: operation2 = "+"; result += number3; break;
        case 1: operation2 = "-"; result -= number3; break;
        case 2: operation2 = "X"; number3 = Random.Range(2, multiDivRange); result *= number3; break;
        case 3: operation2 = "÷"; while (number3 == 0 || result % number3 != 0) { number3 = Random.Range(2, multiDivRange); } result /= number3; break;
    }

    int missingOperation = Random.Range(0, 2);
    if (missingOperation == 0)
    {
        questionText.text = $"{number1} ? {number2} {operation2} {number3} = {result}";
        correctOperation = operation1;
    }
    else
    {
        questionText.text = $"{number1} {operation1} {number2} ? {number3} = {result}";
        correctOperation = operation2;
    }
}

    private void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

    private void PlayCorrectAnswerSound()
    {
        if (correctAnswerSound != null)
        {
            correctAnswerSound.Play();
        }
    }

    private void CheckAnswer(string chosenOperation)
    {
        if (chosenOperation == correctOperation)
        {
            PlayCorrectAnswerSound();
            score += 10;
             UpdateScoreUI(); 
            GenerateQuestion();
        }
        else
        {
            GameOver();
        }
    }

    private void ResetTimer()
    {
        currentTime = timeLimit;
        timeSlider.value = 1;
    }

    private void GameOver()
    {
        isGameOver = true;
        PlayerPrefs.SetInt("FinalScore", score);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Result");
    }

    public int GetScore()
    {
        return score;
    }

        private void UpdateScoreUI()
{
    scoreText.text = "Score: " + score; // Mengupdate tampilan skor
}

}
