using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBandera : MonoBehaviour
{
    [Header("CONFIGURACIÓN")]
    // Solo arrastra el Panel de Victoria en el Nivel 3
    public GameObject canvasVictoria;

    private Animator anim;
    private bool nivelFinalizado = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el que toca la bandera es el jugador y no hemos terminado ya
        if (collision.CompareTag("Player") && !nivelFinalizado)
        {
            nivelFinalizado = true;

            // 1. Animación de la bandera
            if (anim != null) anim.SetTrigger("activar");

            // 2. Parar al jugador para que no siga corriendo
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                // Desactivamos su script de movimiento para que no pueda saltar
                if (collision.TryGetComponent<PlayerMover>(out PlayerMover mover))
                {
                    mover.enabled = false;
                }
            }

            // 3. ¿A dónde vamos?
            // Si estamos en el último nivel (Nivel3), mostramos el panel
            if (SceneManager.GetActiveScene().name == "Nivel3")
            {
                Invoke("MostrarVictoria", 1.5f);
            }
            else
            {
                // Si es Nivel 1 o 2, simplemente saltamos al siguiente tras 2 segundos
                Invoke("CargarSiguienteNivel", 2f);
            }
        }
    }

    void MostrarVictoria()
    {
        // Activamos el cartel de "Has Ganado"
        if (canvasVictoria != null)
        {
            canvasVictoria.SetActive(true);
        }

        // Congelamos el tiempo solo al final del juego
        Time.timeScale = 0f;
    }

    void CargarSiguienteNivel()
    {
        int proximoIndice = SceneManager.GetActiveScene().buildIndex + 1;

        // Si hay una siguiente escena en la lista de Build Settings, la carga
        if (proximoIndice < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(proximoIndice);
        }
    }
}