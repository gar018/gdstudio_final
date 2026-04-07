using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float baseMoveSpeed = 10f;
    public float baseJumpForce = 10f;
    public float hAccelerationScale = 0.1f;

    Vector2 currentVelocity = Vector2.zero;
    bool isGrounded = false;
    InputAction movement;
    InputAction jump;
    //InputAction dash;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        //dash = InputSystem.actions.FindAction("Dash");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerStrafe();
        HandlePlayerJump();
        CheckForGround();
    }

    void HandlePlayerStrafe()
    {
        Vector2 moveInput = movement.ReadValue<Vector2>();
        Vector2 newVelocity;
        
        newVelocity = new Vector2( Mathf.Lerp(rb.linearVelocity.x, baseMoveSpeed * moveInput.x, hAccelerationScale), rb.linearVelocity.y);

        rb.linearVelocity = newVelocity;
    }

    void HandlePlayerJump()
    {
        if (jump.WasPressedThisFrame())
        {
            // Handle jump action
            if (isGrounded)
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

    void CheckForGround()
    {
        // Check if the player is grounded using a raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
