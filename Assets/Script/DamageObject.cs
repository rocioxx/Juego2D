using UnityEngine;

public class DamageObject : MonoBehaviour
{
    // Arrastra aquí un objeto vacío que marque el inicio del nivel [00:04:18]
    public Transform respawnPoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("El jugador ha muerto. Reapareciendo...");
            
            // En lugar de Destroy, movemos al jugador a la posición inicial
            collision.transform.position = respawnPoint.position;

            // Opcional: Si tu jugador tiene físicas, es bueno resetear su velocidad
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}