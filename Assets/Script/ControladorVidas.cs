using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Necesario para la pausa

public class ControladorVidas : MonoBehaviour
{
    [Header("Configuración")]
    public int saludMaxima = 3;
    public int saludActual;

    [Header("Referencias")]
    public Image[] pergaminos;
    public GameObject panelGameOver;
    public TextMeshProUGUI textoPuntuacionFinal;

    [Header("Respawn")]
    public Transform puntoRespawn;

    void Start()
    {
        saludActual = saludMaxima;
        ActualizarUI();
    }

    public void RecibirDano()
    {
        saludActual--;
        ActualizarUI();

        if (saludActual > 0)
        {
            // En lugar de moverlo directo, llamamos a la pausa
            StartCoroutine(PausaAlReaparecer());
        }
        else
        {
            Time.timeScale = 0f;
            panelGameOver.SetActive(true);
        }
    }

    IEnumerator PausaAlReaparecer()
    {
        // 1. Lo movemos al spawn y lo frenamos
        transform.position = puntoRespawn.position;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        // 2. Desactivamos el movimiento
        GetComponent<PlayerMover>().enabled = false;

        // 3. Esperamos 1.5 segundos (ajusta el número si quieres)
        yield return new WaitForSeconds(1.5f);

        // 4. Activamos el movimiento otra vez
        GetComponent<PlayerMover>().enabled = true;
    }

    void ActualizarUI()
    {
        for (int i = 0; i < pergaminos.Length; i++)
        {
            pergaminos[i].enabled = (i < saludActual);
        }
    }
}