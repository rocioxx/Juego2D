using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [Header("ARRASTRA AQUÍ TU OBJETO 'RESPAWN' (INICIO)")]
    public Transform respawnPoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            // 1. AVISAMOS AL MANAGER PARA QUE QUITE VIDA
            ControladorVidas controlador = Object.FindFirstObjectByType<ControladorVidas>();
            if (controlador != null)
            {
                controlador.RecibirDano();
            }

            // 2. ¡IMPORTANTE! TELETRANSPORTAMOS AL JUGADOR AL INICIO SIEMPRE
            // Da igual si tiene 3 vidas o 1, al tocar pincho -> al inicio.
            collision.transform.position = respawnPoint.position;

            // 3. FRENAMOS AL JUGADOR
            // Esto evita que reaparezcas con la inercia del golpe y salgas volando.
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Si usas Unity 6 o muy nuevo usa: rb.linearVelocity = Vector2.zero;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}