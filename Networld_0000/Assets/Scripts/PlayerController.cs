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

    private bool isGrounded;
    private bool isNearLadder;
    private bool isClimbing;
    private bool jumpPressed;
    private bool canToggle;

    private Rigidbody2D rb;
    private Animator anim;
    private float verticalInput;
    private float horizontalInput;

    // SFX state tracking
    private AudioSource sfxSource;
    private bool isLoopingWalk = false;
    private bool isLoopingClimb = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;

        // Sync SFX volume with SoundEffectManager
        var sfxManager = FindFirstObjectByType<SoundEffectManager>();
        if (sfxManager != null)
        {
            // Set initial volume
            sfxSource.volume = sfxManager.GetCurrentVolume();
            // Register for future volume changes
            sfxManager.RegisterSfxVolumeListener(SetSfxVolume);
        }
    }

    private void OnDestroy()
    {
        var sfxManager = FindFirstObjectByType<SoundEffectManager>();
        if (sfxManager != null)
        {
            sfxManager.UnregisterSfxVolumeListener(SetSfxVolume);
        }
    }

    // This method will be called by the SFX manager when the slider changes
    public void SetSfxVolume(float volume)
    {
        if (sfxSource != null)
            sfxSource.volume = volume;
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

        // --- WALK SFX LOOP ---
        bool isWalking = Mathf.Abs(horizontalInput) > 0.1f && isGrounded && !isClimbing;
        if (isWalking && !isLoopingWalk)
        {
            isLoopingWalk = true;
            isLoopingClimb = false;
            PlayRandomLoopingSfx("walk");
        }
        else if (!isWalking && isLoopingWalk)
        {
            isLoopingWalk = false;
            sfxSource.Stop();
        }

        // --- CLIMB SFX LOOP ---
        bool isClimbingMoving = isClimbing && (Mathf.Abs(verticalInput) > 0.1f || Mathf.Abs(horizontalInput) > 0.1f);
        if (isClimbingMoving && !isLoopingClimb)
        {
            isLoopingClimb = true;
            isLoopingWalk = false;
            PlayRandomLoopingSfx("ladder");
        }
        else if ((!isClimbingMoving || !isClimbing) && isLoopingClimb)
        {
            isLoopingClimb = false;
            sfxSource.Stop();
        }

        // When a clip finishes, play another random one if still walking/climbing
        if (!sfxSource.isPlaying && (isLoopingWalk || isLoopingClimb))
        {
            if (isLoopingWalk)
                PlayRandomLoopingSfx("walk");
            else if (isLoopingClimb)
                PlayRandomLoopingSfx("ladder");
        }
    }

    private void PlayRandomLoopingSfx(string groupName)
    {
        var sfxLib = FindFirstObjectByType<SoundEffectLibrary>();
        if (sfxLib == null) return;
        AudioClip clip = sfxLib.GetRandomClip(groupName);
        if (clip != null)
        {
            sfxSource.pitch = 2f; // Increase for faster playback
            sfxSource.clip = clip;
            sfxSource.Play();
        }
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
}