using UnityEngine;

public class Trampolin : MonoBehaviour
{
    [Header("AJUSTES DE SALTO")]
    public float fuerzaSaltoNormal = 12f;  // Primer salto (menos distancia)
    public float fuerzaSuperSalto = 20f;   // Siguientes saltos (más distancia)

    private Animator anim;
    private bool yaSeHaUsadoUnaVez = false; // El interruptor

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos si es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Ponemos la velocidad vertical en 0 para un rebote limpio
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                // COMPROBAMOS EL INTERRUPTOR
                if (!yaSeHaUsadoUnaVez)
                {
                    // === PRIMERA VEZ ===
                    rb.AddForce(Vector2.up * fuerzaSaltoNormal, ForceMode2D.Impulse);
                    Debug.Log("Primer salto: Corta distancia");

                    if (anim != null) anim.SetTrigger("activar");

                    // Activamos el interruptor para que la próxima vez vaya a la otra velocidad
                    yaSeHaUsadoUnaVez = true; 
                }
                else
                {
                    // === SEGUNDA VEZ EN ADELANTE ===
                    rb.AddForce(Vector2.up * fuerzaSuperSalto, ForceMode2D.Impulse);
                    Debug.Log("Siguientes saltos: Gran distancia!");

                    if (anim != null) anim.SetTrigger("activar"); // O "superActivar" si tienes otra
                }
            }
        }
    }
}