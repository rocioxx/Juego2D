using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoBotones : MonoBehaviour
{
    [Header("Movimiento")]
    public float runSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb2D;
    private float horizontalInput;

    [Header("Doble Salto")]
    public int extraJumps = 1; 
    private int jumpsRemaining;

    [Header("Better Jump Settings")]
    public bool betterJump = true;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Componentes")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        jumpsRemaining = extraJumps; 
    }

    void Update()
    {
        // 1. LÓGICA DE TECLADO (PC)
        if (Keyboard.current != null)
        {
            float moveLeft = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1f : 0f;
            float moveRight = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f;
            
            // Solo sobreescribe si hay entrada de teclado para no anular los botones de Android
            if (moveLeft != 0 || moveRight != 0)
            {
                horizontalInput = moveLeft + moveRight;
            }
            else if (Keyboard.current.anyKey.wasReleasedThisFrame) 
            {
                horizontalInput = 0;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                JumpLogic();
            }
        }

        // 2. ANIMACIONES Y GIRO
        HandleVisuals();

        // 3. RECARGA DE SALTOS
        if (CheckGround.isGrounded)
        {
            jumpsRemaining = extraJumps;
        }
    }

    void HandleVisuals()
    {
        if (horizontalInput != 0)
        {
            animator.SetBool("Run", true);
            spriteRenderer.flipX = (horizontalInput < 0);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        animator.SetBool("Jump", !CheckGround.isGrounded);
    }

    // --- FUNCIONES PÚBLICAS PARA BOTONES ANDROID ---

    public void MoverDerecha() => horizontalInput = 1f;
    public void MoverIzquierda() => horizontalInput = -1f;
    public void DetenerMovimiento() => horizontalInput = 0f;

    public void JumpLogic()
    {
        if (CheckGround.isGrounded)
        {
            PerformJump();
        }
        else if (jumpsRemaining > 0)
        {
            PerformJump();
            jumpsRemaining--;
        }
    }

    void PerformJump()
    {
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
    }

    // --- FÍSICAS ---

    void FixedUpdate()
    {
        rb2D.linearVelocity = new Vector2(horizontalInput * runSpeed, rb2D.linearVelocity.y);

        if (betterJump)
        {
            ApplyBetterJump();
        }
    }

    void ApplyBetterJump()
    {
        if (rb2D.linearVelocity.y < 0)
        {
            rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb2D.linearVelocity.y > 0 && (Keyboard.current != null && !Keyboard.current.spaceKey.isPressed))
        {
            rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}