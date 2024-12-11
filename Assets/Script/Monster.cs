using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed = 1.0f; // Kecepatan default untuk monster
    private Vector3 targetPosition; // Posisi target untuk pergerakan monster
    private bool isMoving = true; // Status apakah monster sedang bergerak
    private Animator animator; // Referensi ke Animator monster
    private bool isDying = false; // Status apakah monster sedang mati

    void Start()
    {
        // Menentukan posisi target ke kiri layar
        targetPosition = new Vector3(-7, transform.position.y, 0); // Posisi tujuan monster bergerak ke kiri layar

        // Menyesuaikan kecepatan monster berdasarkan batas waktu dari skrip MathMonsterBattle
        MathMonsterBattle battleScript = GameObject.FindObjectOfType<MathMonsterBattle>();
        if (battleScript != null)
        {
            float timeLimit = battleScript.GetTimeLimit(); // Mengambil batas waktu dari MathMonsterBattle
            SetSpeed(CalculateMonsterSpeed(timeLimit)); // Mengatur kecepatan monster berdasarkan waktu
        }

        // Mengambil referensi ke Animator
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Jika monster masih bergerak, geser posisinya ke arah target
        if (isMoving && !isDying)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            // Jika monster sudah mencapai target di kiri layar
            if (transform.position.x <= -7)
            {
                GameObject.FindObjectOfType<MathMonsterBattle>().GameOver(); // Panggil GameOver dari MathMonsterBattle
                isMoving = false; // Set monster berhenti bergerak
            }
        }
    }

    // Fungsi untuk mengatur kecepatan monster
    public void SetSpeed(float monsterSpeed)
    {
        speed = monsterSpeed;
    }

    // Fungsi untuk menghitung kecepatan monster berdasarkan batas waktu
    private float CalculateMonsterSpeed(float timeLimit)
    {
        // Contoh: semakin kecil batas waktu, semakin cepat monster
        float minSpeed = 1.0f; // Kecepatan paling lambat
        float maxSpeed = 5.0f; // Kecepatan paling cepat
        return Mathf.Lerp(maxSpeed, minSpeed, timeLimit / 10.0f); // Sesuaikan kecepatan monster berdasarkan batas waktu
    }

    // Fungsi untuk memicu animasi mati
    public void Die()
    {
        if (isDying) return; // Jika sudah mati, abaikan pemanggilan berikutnya
        isDying = true; // Set status mati
        isMoving = false; // Hentikan pergerakan
        
        // Memicu animasi mati
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Menghapus objek setelah animasi selesai
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Tunggu durasi animasi mati
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject); // Hancurkan objek monster
    }
}
