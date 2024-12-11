using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelection : MonoBehaviour
{
    private int selectedLevel;                     // Level yang dipilih
    public AudioSource backgroundMusic;            // Musik latar

    private void Start()
    {
        // Memutar musik jika belum diputar, dan mengatur agar tetap berjalan di setiap scene
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;           // Loop musik agar terus dimainkan
            backgroundMusic.Play();
            DontDestroyOnLoad(backgroundMusic.gameObject); // Menjaga musik tetap berjalan di antara scene
        }
    }

    // Fungsi untuk memilih dan memulai game berdasarkan nomor game
    public void SelectGame(int gameNumber)
    {
        // Mendapatkan level yang dipilih dari PlayerPrefs
        selectedLevel = PlayerPrefs.GetInt("SelectedLevel");

        // Memilih dan memuat scene game yang sesuai
        switch (gameNumber)
        {
            case 1:
                LoadGame("MathMonsterBattle");
                break;
            case 2:
                LoadGame("PetFoodMath");
                break;
            case 3:
                LoadGame("TableMath");
                break;
            case 4:
                LoadGame("MathFrenzy");
                break;
            case 5:
                LoadGame("FillTheGap");
                break;
            default:
                Debug.LogError("Invalid game selection"); // Error jika nomor game tidak valid
                break;
        }
    }

    // Fungsi untuk memuat scene game
    private void LoadGame(string gameSceneName)
    {
        // Simpan nama scene ke PlayerPrefs
        PlayerPrefs.SetString("SelectedGameScene", gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    // Fungsi untuk kembali ke scene pemilihan level
    public void BackToLevelSelect()
    {
        // Menghapus level yang dipilih sebelumnya dari PlayerPrefs
        PlayerPrefs.DeleteKey("SelectedLevel");

        // Kembali ke scene PilihLevel
        SceneManager.LoadScene("PilihLevel");
    }
}
