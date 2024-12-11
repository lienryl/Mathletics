using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;       // Panel untuk menampilkan menu pause
    public Button pauseButton;           // Tombol untuk memulai pause
    public Button resumeButton;          // Tombol untuk melanjutkan permainan pada menu pause
    public Button quitButton;            // Tombol untuk keluar dari permainan ke menu utama
    public AudioSource backgroundMusic;  // AudioSource untuk backsound

    private bool isPaused = false;       // Status untuk cek apakah game sedang dipause
    private int pauseCount = 0;          // Penghitung untuk jumlah pause yang digunakan
    private const int maxPauseCount = 3; // Batas maksimum pause yang diizinkan

    void Start()
    {
        // Menambahkan listener pada setiap tombol
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);

        // Menyembunyikan menu pause di awal permainan
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // Opsi tambahan: Mengizinkan pemain mempause/unpause game menggunakan tombol kembali pada perangkat Android
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Fungsi untuk mempause permainan
    public void PauseGame()
    {
        if (pauseCount < maxPauseCount)  // Mengecek apakah jumlah pause masih di bawah batas
        {
            isPaused = true;
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            pauseCount++;  // Meningkatkan penghitung pause setiap kali tombol ditekan

            if (backgroundMusic == null)
            {
                backgroundMusic = FindObjectOfType<AudioSource>(); // Mencari backgroundMusic jika belum diatur
            }

            if (backgroundMusic != null)
            {
                backgroundMusic.Pause();
            }
        }
        else
        {
            Debug.Log("Jumlah maksimum pause telah tercapai.");
            // Kamu bisa menambahkan feedback tambahan di sini jika dibutuhkan
        }
    }

    // Fungsi untuk melanjutkan permainan
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);

        if (backgroundMusic == null)
        {
            backgroundMusic = FindObjectOfType<AudioSource>();
        }

        if (backgroundMusic != null)
        {
            backgroundMusic.UnPause();
        }
    }

    // Fungsi untuk keluar dari permainan dan kembali ke menu utama
    public void QuitGame()
    {
        Time.timeScale = 1; // Memastikan permainan tidak dalam keadaan pause
        SceneManager.LoadScene("MainMenu"); // Memuat ulang ke scene menu utama atau pemilihan level
    }
}
