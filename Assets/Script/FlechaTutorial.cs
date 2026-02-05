using UnityEngine;
using System.Collections; // Necesario para el temporizador

public class FlechaTutorial : MonoBehaviour
{
    [Header("Arrastra aquí el Bocadillo de este mensaje")]
    public GameObject elBocadillo;

    [Header("Tiempo que dura el mensaje")]
    public float duracionMensaje = 3f;

    // Referencias internas
    private SpriteRenderer imagenFlecha;
    private BoxCollider2D colliderFlecha;

    void Start()
    {
        // Cogemos los componentes de la flecha automáticamente
        imagenFlecha = GetComponent<SpriteRenderer>();
        colliderFlecha = GetComponent<BoxCollider2D>();

        // Nos aseguramos de que el bocadillo empiece apagado
        if (elBocadillo != null)
        {
            elBocadillo.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Aquí es donde "conecta" con tu script PlayerMover:
        // Detecta si el objeto tiene el Tag "Player" (tu rana)
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SecuenciaBocadillo());
        }
    }

    // Esta es la magia para esperar sin congelar el juego
    IEnumerator SecuenciaBocadillo()
    {
        // 1. Mostramos el mensaje
        if (elBocadillo != null) elBocadillo.SetActive(true);

        // 2. Ocultamos la flecha visualmente (pero el objeto sigue existiendo un poco más)
        imagenFlecha.enabled = false;

        // 3. Desactivamos el choque para que no se active dos veces
        colliderFlecha.enabled = false;

        // 4. Esperamos los segundos que hayas puesto (ej. 3 segundos)
        yield return new WaitForSeconds(duracionMensaje);

        // 5. Apagamos el mensaje y destruimos la flecha para siempre
        if (elBocadillo != null) elBocadillo.SetActive(false);
        Destroy(gameObject);
    }
}