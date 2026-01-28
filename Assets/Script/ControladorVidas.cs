using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorVidas : MonoBehaviour
{
    [Header("Configuraci�n")]
    public int saludMaxima = 3;
    public int saludActual;

    [Header("Referencias")]
    public Image[] pergaminos;      // Tus vidas
    public GameObject panelGameOver; // El panel de Game Over
    public TextMeshProUGUI textoPuntuacionFinal; // El texto de puntos

    [Header("Respawn / Reaparici�n")]
    public Transform puntoRespawn;  // <--- �NUEVO! Aqu� pondremos el "Spam"

    void Start()
    {
        saludActual = saludMaxima;
        ActualizarUI();
        Time.timeScale = 1f;
    }

    public void RecibirDano()
    {
        saludActual--; // Quitamos una vida
        ActualizarUI(); // Actualizamos los dibujos

        if (saludActual > 0)
        {
            // --- OPCI�N A: A�N EST�S VIVO ---
            // Te devolvemos al "Spam" (Respawn)
            if (puntoRespawn != null)
            {
                transform.position = puntoRespawn.position;

                // IMPORTANTE: Frenamos a la rana para que no salga disparada al reaparecer
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null) rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            // --- OPCI�N B: HAS MUERTO (0 vidas) ---
            // Mostramos puntuaci�n y Game Over
            FruitManager managerFrutas = Object.FindFirstObjectByType<FruitManager>();
            if (managerFrutas != null && textoPuntuacionFinal != null)
            {
                textoPuntuacionFinal.text = "Frutas: " + managerFrutas.collectedFruits;
            }

            Time.timeScale = 0f; // Pausar juego
            panelGameOver.SetActive(true); // Mostrar panel
        }
    }

    void ActualizarUI()
    {
        // Tu c�digo de siempre para los pergaminos
        for (int i = 0; i < pergaminos.Length; i++)
        {
            // CORREGIDO: Usamos la l�gica simple para orden visual (0, 1, 2)
            if (i < saludActual)
            {
                pergaminos[i].enabled = true;
            }
            else
            {
                pergaminos[i].enabled = false;
            }
        }
    }
}