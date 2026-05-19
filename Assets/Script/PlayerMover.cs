using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb2D;
    private float horizontalInput;

    // --- VARIABLES NUEVAS PARA LOS BOTONES MÓVILES ---
    private float inputBotones = 0f;
    private bool botonSaltoPulsado = false;

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
        Time.timeScale = 1f;
        rb2D = GetComponent<Rigidbody2D>();
        jumpsRemaining = extraJumps;
    }

    void Update()
    {
        float moveLeft = 0f;
        float moveRight = 0f;

        // 1. LÓGICA DE TECLADO (Con protección para móviles)
        if (Keyboard.current != null)
        {
            moveLeft = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1f : 0f;
            moveRight = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                IntentarSalto();
            }
        }

        // 2. SUMAMOS EL TECLADO Y LOS BOTONES
        horizontalInput = Mathf.Clamp((moveLeft + moveRight) + inputBotones, -1f, 1f);

        // 3. GIRO Y ANIMACIÓN DE CORRER (Sacado fuera para que funcione en móvil)
        if (horizontalInput != 0)
        {
            animator.SetBool("Run", true);
            spriteRenderer.flipX = (horizontalInput < 0);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        // 4. LÓGICA DE SUELO Y RECARGA
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
    }

    void FixedUpdate()
    {
        rb2D.linearVelocity = new Vector2(horizontalInput * runSpeed, rb2D.linearVelocity.y);

        if (betterJump)
        {
            ApplyBetterJump();
        }
    }

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
        else if (rb2D.linearVelocity.y > 0)
        {
            // Protegemos el "Better Jump" para que detecte tanto el teclado como el botón táctil
            bool espacioPulsado = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;
            if (!espacioPulsado && !botonSaltoPulsado)
            {
                rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
    }

    // --- FUNCIÓN UNIFICADA DE SALTO ---
    void IntentarSalto()
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

    // ====================================================================
    // --- FUNCIONES PÚBLICAS PARA ENLAZAR EN EL EVENT TRIGGER (CANVAS) ---
    // ====================================================================

    public void PulsarIzquierda() { inputBotones = -1f; }
    public void PulsarDerecha() { inputBotones = 1f; }
    public void SoltarMovimiento() { inputBotones = 0f; }

    public void PulsarSalto()
    {
        botonSaltoPulsado = true;
        IntentarSalto();
    }
    public void SoltarSalto() { botonSaltoPulsado = false; }

   private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Caja"))
    {
        // ESTO ES EL CHIVATO: Escribirá un mensaje en la consola de Unity
        Debug.Log("¡SÍ! El jugador ha tocado la caja correctamente");

        JumpBox caja = collision.gameObject.GetComponent<JumpBox>();
        if (caja != null)
        {
            caja.RecibirGolpe();
        }
    }
}
}


