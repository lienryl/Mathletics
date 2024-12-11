using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Metode yang dipanggil ketika arrow bertabrakan dengan objek lain
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Arrow collided with: " + collision.gameObject.name);

        // Mengecek apakah arrow menabrak monster
        if (collision.gameObject.CompareTag("Monster"))
        {
            Debug.Log("Arrow hit the monster!");

            // Memanggil metode `ArrowHitMonster` di dalam script `MathMonsterBattle` untuk menangani kematian monster
            MathMonsterBattle battle = FindObjectOfType<MathMonsterBattle>();
            if (battle != null)
            {
                battle.ArrowHitMonster();
            }  

            // Menghancurkan arrow setelah mengenai monster
            Destroy(gameObject);
        }
    }
}