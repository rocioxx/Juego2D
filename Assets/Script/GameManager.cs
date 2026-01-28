using UnityEngine;
using UnityEngine.SceneManagement; // Para reiniciar nivel
using TMPro; // Para el texto de puntuación

public class GameManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject panelGameOver; // Tu panel de Game Over
    public GameObject hudJuego;      // Tu objeto 'VidaPergamno'
    public TextMeshProUGUI textoPuntuacion; // El texto dentro del Game Over

    // Esta función la llamará la rana al morir
    public void ActivarGameOver()
    {
        // 1. Apagamos las vidas y el contador de frutas
        if (hudJuego != null) hudJuego.SetActive(false);

        // 2. Encendemos el Game Over
        if (panelGameOver != null) panelGameOver.SetActive(true);

        // 3. Ponemos la puntuación
        FruitManager frutas = Object.FindFirstObjectByType<FruitManager>();
        if (frutas != null && textoPuntuacion != null)
        {
            textoPuntuacion.text = "Frutas: " + frutas.collectedFruits;
        }

        // 4. Paramos el juego
        Time.timeScale = 0f;
    }

    // --- BOTONES ---

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f; // Descongelar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f; // Descongelar
        SceneManager.LoadScene("MenuPrincipal"); // Asegúrate que tu menú se llama así
    }
}