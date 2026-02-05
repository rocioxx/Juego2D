using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBandera : MonoBehaviour
{
    private Animator anim;
    private bool nivelFinalizado = false;

    void Start()
    {
        // Obtenemos el componente Animator de la bandera
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto que toca tiene el Tag "Player" y aún no hemos ganado
        if (collision.CompareTag("Player") && !nivelFinalizado)
        {
            nivelFinalizado = true;

            // 1. Lanzamos la animación 'flagout' usando el trigger
            anim.SetTrigger("activar");

            // 2. Opcional: Frenamos a la rana para que no siga moviéndose
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                // Desactivamos el script de movimiento para que no pueda saltar mientras espera
                collision.GetComponent<PlayerMover>().enabled = false;
            }

            // 3. Esperamos a que la animación termine antes de cambiar de escena
            // Ajusta el '1.5f' según lo que dure tu animación
            Invoke("CargarSiguienteNivel", 3f);
        }
    }

    void CargarSiguienteNivel()
    {
        int proximoIndice = SceneManager.GetActiveScene().buildIndex + 1;

        if (proximoIndice < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(proximoIndice);
        }
        else
        {
            Debug.Log("¡Felicidades! Has terminado todos los niveles.");
            // Opcional: Volver al menú principal
            // SceneManager.LoadScene(0);
        }
    }
}