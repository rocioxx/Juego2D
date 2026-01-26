using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb2D;
    private float horizontalInput;

    [Header("Better Jump Settings")]
    public bool betterJump = true;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

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

            // --- GIRO Y ANIMACIÓN DE CORRER ---
            if (horizontalInput != 0)
            {
                animator.SetBool("Run", true);
                spriteRenderer.flipX = (horizontalInput < 0);
            }
            else
            {
                animator.SetBool("Run", false);
            }

            // --- ANIMACIÓN DE SALTO (NUEVO SEGÚN TU IMAGEN) ---
            if (CheckGround.isGrounded == false)
            {
                animator.SetBool("Jump", true);
                animator.SetBool("Run", false);
            }
            if (CheckGround.isGrounded == true)
            {
                animator.SetBool("Jump", false);
            }
            // --------------------------------------------------

            // Salto Inicial
            if (Keyboard.current.spaceKey.wasPressedThisFrame && CheckGround.isGrounded)
            {
                Jump();
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

    void Jump()
    {
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