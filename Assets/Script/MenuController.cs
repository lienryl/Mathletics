using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Komponen audio untuk musik latar dan suara klik tombol
    public AudioSource backgroundMusic;
    public AudioSource buttonClickSound;

    void Start()
    {
        PlayBackgroundMusic(); // Memutar musik latar saat menu dimulai
    }

    // Fungsi untuk menangani tombol "Mulai"
    public void OnStartButtonPressed()
    {
        PlayButtonClickSound(); // Memutar suara klik tombol
        SceneManager.LoadScene("PilihLevel"); // Pindah ke scene pemilihan level
    }

    // Fungsi untuk menangani tombol "Tentang"
    public void OnAboutButtonPressed()
    {
        PlayButtonClickSound(); // Memutar suara klik tombol
        SceneManager.LoadScene("TentangPermainan"); // Pindah ke scene informasi permainan
    }

    // Fungsi untuk memutar musik latar
    private void PlayBackgroundMusic()
    {
        // Cek apakah musik latar tersedia dan belum diputar
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true; // Set ke loop agar terus dimainkan
            backgroundMusic.Play(); // Mulai memutar musik latar
        }
    }

    // Fungsi untuk memutar suara klik tombol
    private void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play(); // Memutar suara klik tombol
        }
    }
}