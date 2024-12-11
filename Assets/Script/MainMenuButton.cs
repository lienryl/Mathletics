using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText; // UI text untuk menampilkan skor akhir
    public Button exitButton;              // Tombol keluar dari permainan
    public AudioSource loseSound;          // Audio source untuk suara kalah

    private void Start()
    {
        // Memutar suara kalah ketika scene hasil dimuat
        if (loseSound != null)
        {
            loseSound.Play();
        }

        // Mengambil skor akhir dari PlayerPrefs dan menampilkannya
        int finalScore = PlayerPrefs.GetInt("FinalScore");
        finalScoreText.text = "Final Score: " + finalScore;

        // Menambahkan listener pada tombol exit untuk menjalankan fungsi ExitGame
        exitButton.onClick.AddListener(ExitGame);
    }

    // Fungsi untuk membersihkan skor akhir dari PlayerPrefs
    private void ExitGame()
    {
        PlayerPrefs.DeleteKey("FinalScore"); // Menghapus skor akhir
    }

    // Fungsi untuk pindah ke scene MainMenu
    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu"); // Memuat scene MainMenu
    }

    // Fungsi untuk memulai ulang game (Retry)
    public void RetryGame()
    {
        // Ambil nama scene dari PlayerPrefs
        string gameSceneName = PlayerPrefs.GetString("SelectedGameScene", "DefaultSceneName"); // Ganti DefaultSceneName jika perlu

        // Menghapus data skor akhir dari PlayerPrefs
        PlayerPrefs.DeleteKey("FinalScore");

        // Memulai ulang scene permainan yang dipilih
        SceneManager.LoadScene(gameSceneName);
    }
}
