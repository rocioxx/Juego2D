using UnityEngine;

public class EnemySpike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            // Buscamos el script de vidas en el jugador
            ControladorVidas vidas = collision.gameObject.GetComponent<ControladorVidas>();

            if (vidas != null)
            {
                // En lugar de DESTROY, llamamos a la función de daño
                vidas.RecibirDano(); 
            }
        }
    }
}