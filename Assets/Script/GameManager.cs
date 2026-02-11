using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena
using TMPro; // Necesario para los textos de puntuación

public class GameManager : MonoBehaviour
{
    [Header("--- PANTALLAS (UI) ---")]
    public GameObject hudJuego;       // La interfaz normal (corazones, puntos)
    public GameObject panelGameOver;  // La pantalla de perder
    public GameObject panelVictoria;  // La pantalla de ganar (Solo para el final)

    [Header("--- PUNTUACIÓN (Truco del Espejo) ---")]
    public TextMeshProUGUI textoPuntosHUD;   // El texto de frutas que se ve al jugar
    public TextMeshProUGUI textoPuntosFinal; // El texto de frutas dentro del Game Over

    [Header("--- ESTADO ---")]
    public bool juegoPausado = false;

    // ---------------------------------------------------------
    // 💀 FUNCIONES DE PERDER (GAME OVER)
    // ---------------------------------------------------------
    public void GameOver()
    {
        Debug.Log("GAME OVER");

        // 1. Copiamos la puntuación de frutas al panel final
        if (textoPuntosHUD != null && textoPuntosFinal != null)
        {
            textoPuntosFinal.text = textoPuntosHUD.text;
        }

        // 2. Ocultamos el juego y mostramos la pantalla de muerte
        if (hudJuego != null) hudJuego.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(true);

        // 3. Congelamos el tiempo
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    // ---------------------------------------------------------
    // 🏆 FUNCIONES DE GANAR (VICTORIA)
    // ---------------------------------------------------------
    public void NivelCompletado()
    {
        Debug.Log(" ¡NIVEL COMPLETADO!");

        // 1. Ocultamos la interfaz de juego
        if (hudJuego != null) hudJuego.SetActive(false);

        // 2. Mostramos el panel de victoria
        if (panelVictoria != null) panelVictoria.SetActive(true);

        // 3. Congelamos el tiempo
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    // ---------------------------------------------------------
    // 🎮 FUNCIONES PARA LOS BOTONES (Clics)
    // ---------------------------------------------------------

    // Botón: Reiniciar Nivel
    public void ReiniciarNivel()
    {
        Time.timeScale = 1f; // Importante: Descongelar el tiempo antes de recargar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Botón: Siguiente Nivel (Carga la siguiente escena en la lista de Build Settings)
    public void SiguienteNivel()
    {
        Time.timeScale = 1f;

        // Calcula el índice de la siguiente escena
        int siguienteIndice = SceneManager.GetActiveScene().buildIndex + 1;

        // Si existe una escena siguiente, la carga
        if (siguienteIndice < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(siguienteIndice);
        }
        else
        {
            Debug.Log("¡No hay más niveles! Volviendo al menú...");
            IrAlMenu();
        }
    }

    // Botón: Ir al Menú Principal
    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal"); // Asegúrate de que tu escena se llame así o pon el índice 0
    }

    // Botón: Salir del Juego
    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}