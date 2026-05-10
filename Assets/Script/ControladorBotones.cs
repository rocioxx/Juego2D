using UnityEngine;

public class ControladorBotones : MonoBehaviour
{
    private PlayerMover rana;

    void Update()
    {
        // Si no tenemos a la rana, buscamos directamente SU SCRIPT, pasando de las etiquetas
        if (rana == null)
        {
            rana = FindObjectOfType<PlayerMover>();

            if (rana != null)
            {
                Debug.Log("✅ ¡El Canvas ha encontrado a la Rana directamente por su script!");
            }
        }
    }

    public void ApretarIzquierda()
    {
        if (rana != null) rana.PulsarIzquierda();
    }

    public void ApretarDerecha()
    {
        if (rana != null) rana.PulsarDerecha();
    }

    public void SoltarTecla()
    {
        if (rana != null) rana.SoltarMovimiento();
    }

    public void ApretarSalto()
    {
        if (rana != null) rana.PulsarSalto();
    }

    public void SoltarSalto()
    {
        if (rana != null) rana.SoltarSalto();
    }
}