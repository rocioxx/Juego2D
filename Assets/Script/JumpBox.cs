using UnityEngine;

public class JumpBox : MonoBehaviour
{
    [Header("Configuracion de Elementos")]
    public GameObject fruit;          // Arrastra aquí tus Bananas_0 (3)
   

    private Animator animator;

    void Start()
    {
        // Conseguimos el componente Animator de la caja automaticamente
        animator = GetComponent<Animator>();
        
        if (fruit != null)
        {
            // 1. Aseguramos que el platano empiece completamente invisible
            fruit.SetActive(false); 

            // 2. Buscamos el FruitManager en la escena y hacemos que el platano sea su hijo
            // Esto sirve para que el contador sume esta fruta al total del nivel automáticamente
            FruitManager manager = FindObjectOfType<FruitManager>();
            if (manager != null)
            {
                fruit.transform.SetParent(manager.transform);
            }
        }
    }

    // ==========================================
    // PASO 1: ESTO LO LLAMA TU PERSONAJE AL GOLPEAR
    // ==========================================
    public void RecibirGolpe()
    {
        if (animator != null)
        {
            // Reproduce la animación de sacudida que creamos (Caja_Hit_Anim)
            animator.Play("caja_golpe"); 
        }
        else
        {
            // Por si acaso no tienes Animator, se rompe directo para que no se trabe el juego
            RomperCajaTotalmente();
        }
    }

    // ==========================================
    // PASO 2: ESTO LO LLAMA EL ANIMATION EVENT (LA ALARMA)
    // ==========================================
    public void RomperCajaTotalmente()
    {
        // 1. Hacemos aparecer el plátano en el aire
        if (fruit != null)
        {
            fruit.SetActive(true);
        }

        // 2. Clonamos los pedacitos de madera flotando en la misma posición de la caja
       
        // 3. Destruimos este objeto de la caja para que desaparezca de la pantalla
        Destroy(gameObject);
    }
}