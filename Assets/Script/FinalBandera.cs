using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBandera : MonoBehaviour
{
    [Header("UI DE VICTORIA")]
    // Arrastra aquí el objeto "Canvas" que está dentro de Victoria
    public GameObject canvasVictoria; 
    // Arrastra aquí el HUD que quieres que desaparezca (corazones, frutas, etc.)
    public GameObject hudJuego;      

    private Animator anim;
    private bool nivelFinalizado = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !nivelFinalizado)
        {
            nivelFinalizado = true;

            // 1. Animación de la bandera
            if (anim != null) anim.SetTrigger("activar");

            // 2. Frenar al jugador (física y controles)
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                if (collision.TryGetComponent<PlayerMover>(out PlayerMover mover))
                {
                    mover.enabled = false;
                }
            }

            // 3. Lógica según el nivel
            // Comprueba si el nombre de la escena es "Nivel3"
            if (SceneManager.GetActiveScene().name == "Nivel3") 
            {
                Invoke("MostrarCanvasFinal", 1.5f);
            }
            else 
            {
                Invoke("CargarSiguienteNivel", 3f);
            }
        }
    }

    void MostrarCanvasFinal()
    {
        // Ocultamos el HUD para que no se superponga
        if (hudJuego != null) hudJuego.SetActive(false);
        
        // Activamos el Canvas de victoria que pediste
        if (canvasVictoria != null) canvasVictoria.SetActive(true);
        
        // Pausamos el juego completamente
        Time.timeScale = 0f;
    }

    void CargarSiguienteNivel()
    {
        int proximoIndice = SceneManager.GetActiveScene().buildIndex + 1;
        if (proximoIndice < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(proximoIndice);
        }
    }
}