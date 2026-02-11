using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControladorVidas : MonoBehaviour
{
    [Header("Configuración")]
    public int saludMaxima = 3;
    public int saludActual;

    [Header("Referencias UI")]
    // IMPORTANTE: Ordena las vidas en el Inspector así: 
    // Element 0: Izquierda | Element 1: Centro | Element 2: Derecha
    public Image[] pergaminos;

    // Ya no necesitamos la referencia directa al panel aquí, 
    // porque el GameManager se encargará de mostrarlo.
    // public GameObject panelGameOver; 

    [Header("Respawn")]
    public Transform puntoRespawn;

    // Escudo para no recibir dos golpes seguidos (Invencibilidad)
    private bool esInvulnerable = false;

    void Start()
    {
        saludActual = saludMaxima;
        ActualizarUI();
    }

    public void RecibirDano()
    {
        // 1. Si ya tenemos el escudo activo, ignoramos el golpe
        if (esInvulnerable) return;

        // 2. Restamos vida
        saludActual--;
        ActualizarUI();

        if (saludActual > 0)
        {
            // --- NO HAS MUERTO AÚN ---
            // Te llevamos al respawn pero SIN pausar el juego
            StartCoroutine(RespawnSinPausa());
        }
        else
        {
            // --- HAS MUERTO (0 Vidas) ---
            // Aquí está el cambio CLAVE: Llamamos al GameManager.
            // Así él puede copiar los puntos de fruta antes de sacar el cartel.

            GameManager gm = Object.FindFirstObjectByType<GameManager>();

            if (gm != null)
            {
                gm.GameOver(); // ¡Jefe, encárgate tú!
            }
            else
            {
                // Por si acaso se te olvidó poner el GameManager en la escena
                Debug.LogError("🚨 ¡No encuentro el script GameManager en la escena!");
                Time.timeScale = 0f; // Al menos paramos el juego
            }
        }
    }

    IEnumerator RespawnSinPausa()
    {
        esInvulnerable = true; // Activamos escudo protector

        // 1. Teletransporte inmediato al punto de respawn
        if (puntoRespawn != null)
        {
            transform.position = puntoRespawn.position;
        }

        // 2. Frenamos el empujón para que no salgas volando, pero NO te quitamos el control
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // 3. Efecto Visual (Parpadeo / Transparencia)
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (spr != null) spr.color = new Color(1f, 1f, 1f, 0.5f); // 50% transparente

        // 4. Esperamos 2 segundos de inmunidad (puedes moverte mientras)
        yield return new WaitForSeconds(2.0f);

        // 5. Quitamos el escudo y volvemos a color normal
        if (spr != null) spr.color = Color.white;
        esInvulnerable = false;
    }

    void ActualizarUI()
    {
        // Recorremos las imágenes de los pergaminos/corazones
        for (int i = 0; i < pergaminos.Length; i++)
        {
            if (pergaminos[i] != null)
            {
                // Si tienes 2 vidas:
                // i=0 (Izq) < 2 --> TRUE (Encendido)
                // i=1 (Cen) < 2 --> TRUE (Encendido)
                // i=2 (Der) < 2 --> FALSE (Apagado)
                pergaminos[i].enabled = (i < saludActual);
            }
        }
    }
}