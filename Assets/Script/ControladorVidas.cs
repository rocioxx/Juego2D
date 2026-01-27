using UnityEngine;
using UnityEngine.UI;
using TMPro; // Para el texto

public class ControladorVidas : MonoBehaviour
{
    public int saludMaxima = 3;
    public int saludActual;

    [Header("UI Referencias")]
    public Image[] pergaminos;       // Tus 3 iconos
    public GameObject panelGameOver; // El panel que creamos
    public TextMeshProUGUI textoPuntuacion; // El texto de frutas del Game Over

    void Start()
    {
        saludActual = saludMaxima;
        Time.timeScale = 1f; // Aseguramos que el juego corra al empezar
        ActualizarUI();
    }

    public void RecibirDano()
    {
        saludActual--; // Resta 1 vida
        ActualizarUI(); // Actualiza los dibujos

        // SI LLEGAMOS A 0... GAME OVER
        if (saludActual <= 0)
        {
            // 1. Buscamos la puntuación de las frutas
            FruitManager frutas = Object.FindFirstObjectByType<FruitManager>();
            if (frutas != null && textoPuntuacion != null)
            {
                textoPuntuacion.text = "Frutas: " + frutas.collectedFruits;
            }

            // 2. Activamos el panel de Game Over
            if (panelGameOver != null) panelGameOver.SetActive(true);

            // 3. Congelamos el juego
            Time.timeScale = 0f;
        }
    }

    void ActualizarUI()
    {
        // Muestra u oculta pergaminos según la vida
        for (int i = 0; i < pergaminos.Length; i++)
        {
            pergaminos[i].enabled = (i < saludActual);
        }
    }
}