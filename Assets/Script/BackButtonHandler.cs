using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    public AudioSource backgroundMusic;   // Audio source untuk musik latar
    public AudioSource buttonClickSound;  // Audio source untuk suara klik tombol

    void Start()
    {
        PlayBackgroundMusic();  // Memainkan musik latar saat scene dimulai
    }

    // Metode untuk kembali ke scene "MainHome"
    public void GoBackToMainScene()
    {
        PlayButtonClickSound();  // Memainkan suara klik tombol
        SceneManager.LoadScene("MainHome");
    }

    // Metode untuk kembali ke scene "PilihLevel"
    public void GoBackToLevelScene()
    {
        PlayButtonClickSound();  // Memainkan suara klik tombol
        SceneManager.LoadScene("PilihLevel");
    }

    // Memainkan musik latar jika tidak sedang dimainkan
    private void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;  // Set ke loop agar musik dimainkan terus-menerus
            backgroundMusic.Play();
        }
    }

    // Memainkan suara klik tombol saat tombol ditekan
    private void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
    }
}