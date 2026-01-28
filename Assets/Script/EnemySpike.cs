using UnityEngine;

public class EnemySpike : MonoBehaviour
{
    // Si el enemigo es sólido (Rana choca contra él)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Atacar(collision.gameObject);
        }
    }

    // Si el enemigo es fantasma/trigger (Rana lo atraviesa)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Atacar(collision.gameObject);
        }
    }

    void Atacar(GameObject jugador)
    {
        // Buscamos el script que YA TIENES BIEN PUESTO en la rana
        ControladorVidas vida = jugador.GetComponent<ControladorVidas>();

        if (vida != null)
        {
            vida.RecibirDano(); // ¡Golpe!
        }
    }
}