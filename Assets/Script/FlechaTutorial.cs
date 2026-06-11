using UnityEngine;
using System.Collections; // Necesario para el temporizador

public class FlechaTutorial : MonoBehaviour
{
    [Header("CONFIGURACIÓN DE UI")]
    [Tooltip("Arrastra aquí el objeto del Bocadillo (hijo del Canvas)")]
    public GameObject elBocadillo;

    [Header("TIEMPOS")]
    [Tooltip("¿Cuántos segundos se queda el mensaje en pantalla?")]
    public float duracionMensaje = 3f;

    // Referencia al componente de choque
    private BoxCollider2D colliderFlecha;

    void Start()
    {
        colliderFlecha = GetComponent<BoxCollider2D>();

        // 1. Verificamos que el bocadillo esté asignado para evitar errores
        if (elBocadillo != null)
        {
            elBocadillo.SetActive(false);
        }
        else
        {
            // Cambiado a LogWarning para evitar que Unity congele la interfaz y los botones
            Debug.LogWarning("⚠️ AVISO: ¡No has arrastrado el Bocadillo al script en el Inspector de " + gameObject.name + "!");
        }

        // 2. Comprobación de seguridad del Collider
        if (colliderFlecha == null)
        {
            Debug.LogWarning("⚠️ AVISO: El objeto " + gameObject.name + " no tiene un BoxCollider2D.");
        }
        else if (!colliderFlecha.isTrigger)
        {
            Debug.LogWarning("⚠️ AVISO: El BoxCollider2D no tiene marcado 'Is Trigger'. ¡Actívalo!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // --- SISTEMA DE DETECTIVE ---
        // Esto saldrá en la consola SIEMPRE que algo toque la flecha
        Debug.Log("🔍 ALGO HA TOCADO LA FLECHA: " + other.gameObject.name + " (Tag: " + other.tag + ")");

        // Detecta si el objeto tiene el Tag "Player" (tu rana)
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ ¡JUGADOR DETECTADO! Iniciando bocadillo...");
            StartCoroutine(SecuenciaBocadillo());
        }
    }

    IEnumerator SecuenciaBocadillo()
    {
        // 1. Mostramos el mensaje
        if (elBocadillo != null) elBocadillo.SetActive(true);

        // 2. Desactivamos el choque para que no se active dos veces mientras se lee
        if (colliderFlecha != null) colliderFlecha.enabled = false;

        // 3. Esperamos el tiempo definido
        yield return new WaitForSeconds(duracionMensaje);

        // 4. Apagamos el mensaje
        if (elBocadillo != null) elBocadillo.SetActive(false);

        // 5. Destruimos la flecha del mundo para limpiar la escena
        Debug.Log("🗑️ Tutorial finalizado, destruyendo flecha.");
        Destroy(gameObject);
    }
}