using UnityEngine;

public class Trampolin : MonoBehaviour
{
    [Header("AJUSTES")]
    public float fuerzaSalto = 15f; // Ajusta este valor para saltar más o menos

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos si lo que tocó el trampolín es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // 1. Ponemos la velocidad vertical en 0 para que el rebote sea constante
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                // 2. Aplicamos la fuerza hacia arriba
                rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

                // 3. Activamos la animación (si tienes una)
                if (anim != null)
                {
                    anim.SetTrigger("activar"); 
                }
            }
        }
    }
}