using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb2D;
    private float horizontalInput;

    [Header("Doble Salto")]
    // Si quieres más saltos en el aire, aumenta este número
    public int extraJumps = 1; 
    private int jumpsRemaining;

    [Header("Better Jump Settings")]
    public bool betterJump = true;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        jumpsRemaining = extraJumps; 
    }
     
    void Update()
    {
        if (Keyboard.current != null)
        {
            // Movimiento Horizontal
            float moveLeft = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1f : 0f;
            float moveRight = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f;
            horizontalInput = moveLeft + moveRight;

            // Giro y Animación de Correr
            if (horizontalInput != 0)
            {
                animator.SetBool("Run", true);
                spriteRenderer.flipX = (horizontalInput < 0);
            }
            else
            {
                animator.SetBool("Run", false);
            }

            // --- LÓGICA DE SUELO Y RECARGA ---
            if (CheckGround.isGrounded)
            {
                animator.SetBool("Jump", false);
                jumpsRemaining = extraJumps; // Recargamos el salto extra al tocar suelo
            }
            else
            {
                animator.SetBool("Jump", true);
                animator.SetBool("Run", false);
            }

            // --- LÓGICA DE SALTO MODIFICADA ---
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (CheckGround.isGrounded)
                {
                    // Salto normal desde el suelo
                    PerformJump();
                }
                else if (jumpsRemaining > 0)
                {
                    // Salto extra en el aire
                    PerformJump();
                    jumpsRemaining--; // Solo restamos si es en el aire
                }
            }
        }
    }

    void FixedUpdate()
    {
        rb2D.linearVelocity = new Vector2(horizontalInput * runSpeed, rb2D.linearVelocity.y);

        if (betterJump)
        {
            ApplyBetterJump();
        }
    }

    // He renombrado la función para que sea más clara
    void PerformJump()
    {
        // Reseteamos la velocidad en Y para que el segundo salto no pierda fuerza si estamos cayendo
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
    }

    void ApplyBetterJump()
    {
        if (rb2D.linearVelocity.y < 0)
        {
            rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb2D.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}