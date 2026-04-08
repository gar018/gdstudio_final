using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float baseMoveSpeed = 5f;
    [SerializeField]
    private float baseJumpForce = 7.5f;
    [SerializeField]
    private float baseDashForce = 10f;
    [SerializeField]
    private float baseDashCooldown = 1f;
    [SerializeField]
    private float hAccelerationScale = 0.003f;

    //public static event Action<Vector2> OnPlayerJump;
    //public static event Action<Vector2> OnPlayerDash;

    Vector2 currentVelocity = Vector2.zero;

    bool isGrounded = false;
    Transform groundCheckTransform;
    float dashCooldownTimer = 0f;
    bool dashReady = true;
    InputAction movement;
    InputAction jump;
    InputAction dash;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        dash = InputSystem.actions.FindAction("Dash");
        rb = GetComponent<Rigidbody2D>();
        groundCheckTransform = transform.Find("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerStrafe();
        HandlePlayerDash();
        HandlePlayerJump();
        DecrementCooldowns();
        GroundCheck();
    }
    void DecrementCooldowns()
    {
        if (!dashReady)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0f)
            {
                dashReady = true;
            }
        }
    }

    void HandlePlayerStrafe()
    {
        Vector2 moveInput = movement.ReadValue<Vector2>();
        Vector2 newVelocity;
        
        newVelocity = new Vector2( Mathf.Lerp(rb.linearVelocity.x, baseMoveSpeed * moveInput.x, hAccelerationScale), rb.linearVelocity.y);

        rb.linearVelocity = newVelocity;
    }

    void HandlePlayerDash()
    {
        if (dash.WasPressedThisFrame() && dashReady)
        {
            // Handle dash action
            Vector2 dashDirection = new Vector2(movement.ReadValue<Vector2>().x, 0).normalized;
            if (dashDirection != Vector2.zero)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Reset horizontal velocity before dashing
                rb.AddForce(dashDirection * baseDashForce, ForceMode2D.Impulse);
                // Start cooldown timer
                dashCooldownTimer = baseDashCooldown;
                dashReady = false;
            }
            //TODO handle dash direction when no movement input is given (dash in facing direction)
        }
    }

    void HandlePlayerJump()
    {
        if (jump.WasPressedThisFrame() && isGrounded)
        {
            // Handle jump action
            if (1 == 1) //TODO actually deal with ground checking
            {
                rb.AddForce(new Vector2(0, baseJumpForce), ForceMode2D.Impulse);
            }
        }
        else
        {
           // Handle jump release (if needed)
           if (rb.linearVelocity.y > 0 && jump.WasReleasedThisFrame())
            {
                // Reduce the upward velocity when the jump button is released
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }

    void GroundCheck()
    {
        Vector3 origin = groundCheckTransform.position - Vector3.up * 0.001f; // Slightly above the ground check position to avoid self-collision
        isGrounded = Physics2D.Raycast(origin, Vector2.down, 0.15f);
    }
    //add a debug gizmo to show the ground check raycast
    void OnDrawGizmosSelected()
    {   
        if (groundCheckTransform != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(groundCheckTransform.position, groundCheckTransform.position + Vector3.down * 0.15f);
        } 
    }
}
