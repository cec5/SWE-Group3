using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D cd;

    [Header("Movement Settings")]
    private float xInput;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float crouchSpeed = 3f;
    private float currentSpeed;

    [SerializeField] private float jumpForce = 12f;
    private bool facingRight = true;

    [Header("States")]
    private bool isRunning;
    private bool isCrouching;

    [Header("Collision Details")]
    [SerializeField] private LayerMask whatIsGround;

    [Space]
    private Vector2 groundCheckSize = new Vector2(0.7f, 0.2f);
    [SerializeField] private float groundCheckOffset = -0.92f;
    private bool isGrounded;

    [Space]
    private Vector2 ceilingCheckSize = new Vector2(0.7f, 0.2f);
    [SerializeField] private float ceilingCheckOffset = 0.23f;
    private bool headBlocked;

    [Header("Collider Settings")]
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    [SerializeField] private float crouchSizeMultiplier = 0.72f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        cd = GetComponent<CapsuleCollider2D>();
        originalColliderSize = cd.size;
        originalColliderOffset = cd.offset;
    }

    private void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
        AdjustCollider();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);
        bool crouchInput = isGrounded && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));

        if (crouchInput)
        {
            isCrouching = true;
        }
        else if (!headBlocked)
        {
            isCrouching = false;
        }

        if (!isCrouching && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Jump();
        }
    }

    private void HandleMovement()
    {
        if (isCrouching) currentSpeed = crouchSpeed;
        else if (isRunning) currentSpeed = runSpeed;
        else currentSpeed = walkSpeed;

        rb.linearVelocity = new Vector2(xInput * currentSpeed, rb.linearVelocity.y);
    }

    private void AdjustCollider()
    {
        if (isCrouching)
        {
            cd.size = new Vector2(originalColliderSize.x, originalColliderSize.y * crouchSizeMultiplier);
            float offsetShift = (originalColliderSize.y - cd.size.y) / 2f;
            cd.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - offsetShift);
        }
        else
        {
            cd.size = originalColliderSize;
            cd.offset = originalColliderOffset;
        }
    }

    private void HandleCollision()
    {
        Vector2 groundCheckPos = (Vector2)transform.position + Vector2.up * groundCheckOffset;
        isGrounded = Physics2D.OverlapBox(groundCheckPos, groundCheckSize, 0, whatIsGround);

        Vector2 ceilingCheckPos = (Vector2)transform.position + Vector2.up * ceilingCheckOffset;
        headBlocked = Physics2D.OverlapBox(ceilingCheckPos, ceilingCheckSize, 0, whatIsGround);
    }

    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isCrouching", isCrouching);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void HandleFlip()
    {
        if (rb.linearVelocity.x > 0.1f && !facingRight) Flip();
        else if (rb.linearVelocity.x < -0.1f && facingRight) Flip();
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector2 groundCheckPos = (Vector2)transform.position + Vector2.up * groundCheckOffset;
        Gizmos.DrawWireCube(groundCheckPos, groundCheckSize);

        Vector2 ceilingCheckPos = (Vector2)transform.position + Vector2.up * ceilingCheckOffset;
        Gizmos.DrawWireCube(ceilingCheckPos, ceilingCheckSize);
    }
}