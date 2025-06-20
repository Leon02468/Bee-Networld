using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private LadderHandlers ladder = null;
    private HoneyBit honeyBit = null;

    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce;
    [SerializeField] public float climbSpeed;
    [SerializeField] private TextMeshProUGUI beeText;
    [SerializeField] private TextMeshProUGUI playerText;

    private bool isGrounded;
    private bool isNearLadder;
    private bool isClimbing;
    private bool jumpPressed;
    private bool canToggle;

    private Rigidbody2D rb;
    private Animator anim;
    private float verticalInput;
    private float horizontalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isClimbing)
        {
            rb.linearVelocity = new Vector2(0, verticalInput * climbSpeed);

            if (jumpPressed)
            {
                jumpPressed = false;
                StopClimbing();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            if (verticalInput < -0.1f && (transform.position.y <= ladder.GetBottomY() + 1f))
            {
                StopClimbing();
                rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
            }
            else if (verticalInput > 0.1f && (transform.position.y >= ladder.GetTopY() - 1f))
            {
                StopClimbing();
                rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
            }
        }
        else
        {
            rb.gravityScale = 8f;
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }

        anim.SetBool("isJumping", !isGrounded && !isClimbing);
        anim.SetBool("isClimbing", !isGrounded && isClimbing);
    }

    private void FixedUpdate()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocityX));

        if (!isGrounded)
        {
            if (isClimbing)
                anim.SetFloat("yVelocity", Mathf.Abs(rb.linearVelocityY));
            else
                anim.SetFloat("yVelocity", rb.linearVelocityY);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Removed isWalking animation parameter
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocityY = jumpForce;
        }

        if (context.performed && isClimbing)
        {
            jumpPressed = true;
        }
    }

    public void Climb(InputAction.CallbackContext context)
    {
        verticalInput = context.ReadValue<Vector2>().y;
        if (isNearLadder && Mathf.Abs(verticalInput) > 0.1f && !isClimbing)
        {
            StartClimbing();
            rb.linearVelocity = Vector2.zero;

            if (ladder != null)
            {
                float ladderCenterX = ladder.getCenterX();
                transform.position = new Vector3(ladderCenterX, transform.position.y, transform.position.z);
            }
        }
    }

    public void ToggleHoney(InputAction.CallbackContext context)
    {
        if (context.performed && canToggle && honeyBit != null)
        {
            honeyBit.Toggle();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Honey"))
        {
            honeyBit = collision.GetComponentInParent<HoneyBit>();
            canToggle = true;
        }

        if (collision.CompareTag("Ladder"))
        {
            isNearLadder = true;
            ladder = collision.GetComponent<LadderHandlers>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Honey"))
        {
            honeyBit = null;
            canToggle = false;
        }

        if (collision.CompareTag("Ladder"))
        {
            isNearLadder = false;
            ladder = null;
            StopClimbing();
        }
    }

    public void StartClimbing()
    {
        isClimbing = true;
        rb.gravityScale = 0f;
    }
    public void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 8f;
    }

    // --- Reset all movement variables and velocity when disabling ---
    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.Sleep();
        }
        horizontalInput = 0f;
        verticalInput = 0f;
        isClimbing = false;
        jumpPressed = false;
    }
}