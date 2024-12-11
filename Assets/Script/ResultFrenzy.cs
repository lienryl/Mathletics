using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultFrenzy : MonoBehaviour
{
    public TextMeshProUGUI scoreText;    // Teks untuk menampilkan skor akhir
    public TextMeshProUGUI timeText;     // Teks untuk menampilkan waktu total
    public AudioSource loseSound;        // Efek suara ketika pemain kalah

    private void Start()
    {
        // Memainkan efek suara kekalahan jika ada
        if (loseSound != null)
        {
            loseSound.Play();
        }

        // Mendapatkan skor akhir dan total waktu dari PlayerPrefs
        int finalScore = PlayerPrefs.GetInt("FinalScore");
        float totalTime = PlayerPrefs.GetFloat("TotalTime");

        // Menampilkan skor akhir jika elemen UI tersedia
        if (scoreText != null)
        {
            scoreText.text = "Final Score: " + finalScore;
        }
        else
        {
            Debug.LogError("scoreText belum ditetapkan di inspector.");
        }

        // Menampilkan waktu total jika elemen UI tersedia
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(totalTime / 60F);  // Menghitung menit
            int seconds = Mathf.FloorToInt(totalTime % 60F);  // Menghitung detik
            timeText.text = $"Time: {minutes:00}:{seconds:00}";
        }
        else
        {
            Debug.LogError("timeText belum ditetapkan di inspector.");
        }
    }

    // Fungsi untuk berpindah ke scene menu utama
    public void GoToMainMenuScene()
    {
        // Menghapus data skor akhir dan waktu dari PlayerPrefs
        PlayerPrefs.DeleteKey("FinalScore");
        PlayerPrefs.DeleteKey("TotalTime");

        // Berpindah ke scene menu utama
        SceneManager.LoadScene("MainMenu");
    }

    // Fungsi untuk memulai ulang game (Retry)
    public void RetryGame()
    {
        // Ambil nama scene dari PlayerPrefs
        string gameSceneName = PlayerPrefs.GetString("SelectedGameScene", "DefaultSceneName"); // Ganti DefaultSceneName jika perlu

        // Menghapus data skor akhir dan waktu dari PlayerPrefs
        PlayerPrefs.DeleteKey("FinalScore");
        PlayerPrefs.DeleteKey("TotalTime");

        // Memulai ulang scene permainan yang dipilih
        SceneManager.LoadScene(gameSceneName);
    }
}
