using UnityEngine;

public class CheckGround : MonoBehaviour
{
    // Al ser static, PlayerMover puede leerlo fácilmente
    public static bool isGrounded;

    // Usamos un contador para evitar errores cuando hay múltiples plataformas
    private int platformsTouching = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Es recomendable que tu suelo tenga el Tag "Ground"
        if (collision.CompareTag("Ground"))
        {
            platformsTouching++;
            UpdateGrounded();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            platformsTouching--;
            UpdateGrounded();
        }
    }

    private void UpdateGrounded()
    {
        // Si tocamos 1 o más plataformas, estamos en el suelo
        isGrounded = platformsTouching > 0;
    }
}