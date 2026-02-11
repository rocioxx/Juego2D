using UnityEngine;

public class FruitCollected : MonoBehaviour
{
    // --- ESTA ES LA CLAVE ---
    // Un interruptor para recordar si ya hemos chocado
    private bool yaRecogida = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si toca al Jugador... Y ADEMÁS el interruptor está apagado
        if (collision.CompareTag("Player") && !yaRecogida)
        {
            // 1. ¡CERRAMOS EL INTERRUPTOR!
            // Ahora, si viene el segundo collider de los pies, ya no podrá entrar aquí
            yaRecogida = true;

            // 2. Avisamos al Manager (solo una vez)
            FruitManager manager = Object.FindFirstObjectByType<FruitManager>();

            if (manager != null)
            {
                manager.AddFruit();
                Debug.Log("Fruta recogida: " + gameObject.name);
            }

            // 3. Destruimos la fruta
            Destroy(gameObject);

            // Si usas animación, usa esto en vez de Destroy directo:
            // GetComponent<Animator>().SetTrigger("collected");
            // Destroy(gameObject, 0.5f);
        }
    }
}