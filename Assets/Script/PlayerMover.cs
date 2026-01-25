using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 7f; // Cambiado a un nombre más claro

    private Rigidbody2D rb2D;
    private float horizontalInput;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movimiento Horizontal con el Nuevo Input System
        if (Keyboard.current != null)
        {
            float moveLeft = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1f : 0f;
            float moveRight = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f;
            horizontalInput = moveLeft + moveRight;

            // SALTO: Usamos Keyboard.current.spaceKey en lugar de Input.GetKey
            // Nota: Asegúrate de que tu script 'CheckGround' sea estático o esté accesible
            if (Keyboard.current.spaceKey.wasPressedThisFrame && CheckGround.isGrounded)
            {
                Jump();
            }
        }
    }

    void FixedUpdate()
    {
        // Aplicamos la velocidad lineal (desplazamiento), NO la angular (rotación)
        rb2D.linearVelocity = new Vector2(horizontalInput * runSpeed, rb2D.linearVelocity.y);
    }

    void Jump()
    {
        // Esta es la forma correcta de aplicar el salto en 2D
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
    }
}