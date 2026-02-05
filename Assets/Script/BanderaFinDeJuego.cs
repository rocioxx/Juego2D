using UnityEngine;

public class BanderaFinDeJuego : MonoBehaviour
{
    [Header("CONFIGURACIÓN FINAL")]
    public GameObject panelVictoria; // Arrastra aquí la imagen de "Has Ganado"
    public GameObject hudJuego;      // Arrastra aquí el Canvas de corazones para ocultarlo

    private Animator anim;
    private bool juegoTerminado = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si toca el jugador y no hemos terminado aún
        if (collision.CompareTag("Player") && !juegoTerminado)
        {
            juegoTerminado = true;

            // 1. Animación de la bandera
            if (anim != null) anim.SetTrigger("activar");

            // 2. Frenamos a la rana (Física y Script)
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero; // Frenamos en seco

                // Desactivamos el movimiento para que no salte más
                MonoBehaviour mover = collision.GetComponent<PlayerMover>();
                if (mover != null) mover.enabled = false;
            }

            // 3. Esperamos 1 segundo y sacamos el cartel FINAL
            Invoke("MostrarPantallaFinal", 1f);
        }
    }

    void MostrarPantallaFinal()
    {
        // Ocultamos la interfaz de juego
        if (hudJuego != null) hudJuego.SetActive(false);

        // Mostramos el cartel de ¡HAS GANADO!
        if (panelVictoria != null) panelVictoria.SetActive(true);

        // Congelamos el juego
        Time.timeScale = 0f;
    }
}