using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // Audio source untuk musik
    public AudioSource buttonClickSound; // Audio source untuk suara tombol

    void Start()
    {
        // Memulai musik jika belum diputar
        PlayBackgroundMusic();
    }

    // Fungsi untuk memainkan musik dengan loop
    private void PlayBackgroundMusic()
    {
        // Mengecek apakah musik tersedia dan belum diputar
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true; // Mengatur musik untuk diputar terus menerus
            backgroundMusic.Play(); // Memutar musik
        }
    }

    // Fungsi untuk memainkan efek suara tombol klik
    private void PlayButtonClickSound()
    {
        // Mengecek apakah suara klik tombol tersedia
        if (buttonClickSound != null)
        {
            buttonClickSound.Play(); // Memutar suara klik tombol
        }
    }

    // Fungsi untuk pindah ke scene MainHome
    public void GoToSceneMainHome()
    {
        PlayButtonClickSound(); // Memainkan suara tombol klik
        SceneManager.LoadScene("MainHome"); // Memuat scene MainHome
    }

    // Fungsi untuk pindah ke scene PilihLevel
    public void GoToSceneLevelSelect()
    {
        PlayButtonClickSound(); // Memainkan suara tombol klik
        SceneManager.LoadScene("PilihLevel"); // Memuat scene PilihLevel
    }

    // Fungsi untuk memilih level dan menyimpannya di PlayerPrefs
    public void SelectLevel(int level)
    {
        PlayButtonClickSound(); // Memainkan suara tombol klik
        PlayerPrefs.SetInt("SelectedLevel", level); // Menyimpan level yang dipilih di PlayerPrefs
        
        // Pindah ke scene MainMenu untuk pemilihan mini-game
        SceneManager.LoadScene("MainMenu");
    }
}