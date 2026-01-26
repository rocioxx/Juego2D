using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 10f; // Fuerza del salto inicial

    private Rigidbody2D rb2D;
    private float horizontalInput;

    [Header("Better Jump Settings")]
    public bool betterJump = true;
    public float fallMultiplier = 2.5f;      // Gravedad al caer
    public float lowJumpMultiplier = 2f;    // Gravedad si sueltas rápido el espacio

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            // Movimiento Horizontal
            float moveLeft = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1f : 0f;
            float moveRight = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f;
            horizontalInput = moveLeft + moveRight;

            // Salto Inicial
            if (Keyboard.current.spaceKey.wasPressedThisFrame && CheckGround.isGrounded)
            {
                Jump();
            }
        }
    }

    void FixedUpdate()
    {
        // Aplicar movimiento horizontal
        rb2D.linearVelocity = new Vector2(horizontalInput * runSpeed, rb2D.linearVelocity.y);

        // Lógica de Better Jump (Caída mejorada)
        if (betterJump)
        {
            ApplyBetterJump();
        }
    }

    void Jump()
    {
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
    }

    void ApplyBetterJump()
    {
        // Si estamos cayendo (velocidad Y negativa)
        if (rb2D.linearVelocity.y < 0)
        {
            // Cae más rápido: suma gravedad extra
            rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Si estamos subiendo pero NO estamos pulsando espacio (salto corto)
        else if (rb2D.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            // Reduce la subida más rápido: suma gravedad extra
            rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}