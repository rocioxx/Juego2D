using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [Header("Arrastra aquí tu objeto 'Respawn'")]
    public Transform respawnPoint;

    // Opción A: Si chocan como sólidos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            GolpearJugador(collision.transform);
        }
    }

    // Opción B: Si el enemigo es un Trigger (fantasma)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GolpearJugador(collision.transform);
        }
    }

    // La función que hace el daño y teletransporta
    void GolpearJugador(Transform jugador)
    {
        // 1. Quitar vida
        ControladorVidas controlador = Object.FindFirstObjectByType<ControladorVidas>();
        if (controlador != null)
        {
            controlador.RecibirDano();
        }

        // 2. Teletransportar al inicio
        if (respawnPoint != null)
        {
            jugador.position = respawnPoint.position;

            // 3. Frenar el golpe (para que no salga disparado)
            Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // Frenar en seco
            }
        }
        else
        {
            Debug.LogWarning("¡Se te olvidó poner el Respawn Point en el enemigo!");
        }
    }
}