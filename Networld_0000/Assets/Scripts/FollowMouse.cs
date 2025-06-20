using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowMouse : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;
    private Animator anim;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // No gravity for side-to-side movement
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Get mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float direction = mousePosition.x - transform.position.x;

        // Determine movement direction (-1, 0, 1)
        moveInput.x = Mathf.Clamp(direction, -1f, 1f);

        // Flip sprite based on direction
        if (moveInput.x > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        // Only move horizontally, let physics handle collisions
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, 0f);

        // Set walking animation
        if (anim != null)
        {
            anim.SetFloat("xVelocity", Mathf.Abs(moveInput.x));
        }
    }
}