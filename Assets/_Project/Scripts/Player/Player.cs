using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D cd;

    [Header("Movement Settings")]
    public float xInput;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float crouchSpeed = 3f;
    private float currentSpeed;

    [SerializeField] public int extraJumpsValue = 1;
    private int extraJumps;
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
    public bool isGrounded;

    [Space]
    private Vector2 ceilingCheckSize = new Vector2(0.7f, 0.2f);
    [SerializeField] private float ceilingCheckOffset = 0.23f;
    private bool headBlocked;

    [Header("Collider Settings")]
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    [SerializeField] private float crouchSizeMultiplier = 0.72f;

    [Header("Lives & Respawn")]
    [SerializeField] private TextMeshProUGUI PlayerLivesText;
    [SerializeField] private int maxLives = 3;
    private int currentLives;
    public GameObject[] hearts;
    private Vector2 currentRespawnPosition;
    public static event Action OnPlayerRespawn; // Not needed now, but can be used in the future to reset lava or other variables on respawn

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        cd = GetComponent<CapsuleCollider2D>();
        originalColliderSize = cd.size;
        originalColliderOffset = cd.offset;
        extraJumps = extraJumpsValue;

        currentLives = maxLives;
        currentRespawnPosition = transform.position;
        UpdateLivesUI();
    }

    private void Update()
    {
        if (PauseControl.IsPaused)
        {
            return;
        }

        
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector2 groundCheckPos = (Vector2)transform.position + Vector2.up * groundCheckOffset;
        Gizmos.DrawWireCube(groundCheckPos, groundCheckSize);

        Vector2 ceilingCheckPos = (Vector2)transform.position + Vector2.up * ceilingCheckOffset;
        Gizmos.DrawWireCube(ceilingCheckPos, ceilingCheckSize);
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
            extraJumps = extraJumpsValue;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        } else if (extraJumps > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            extraJumps--;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard")) // This was a lot easier than expected, just tag the appropriate objects
        {
            LoseLife();
        }
        else if (collision.CompareTag("End")) // player reaches the end
        {
            GameWon();
        }
    }
    private void LoseLife()
    {
        // Remove heart
        if (currentLives - 1 > 0 && currentLives - 1 < hearts.Length)
        {
            Destroy(hearts[currentLives - 1].gameObject);
        }
        currentLives--;
        if (currentLives > 0)
        {
            Respawn();
        }
        else // Resets the scene entirely if all lives are lost
        {
            // Death/ restart menu
            SceneManager.LoadScene(1);
        }
        UpdateLivesUI();
    }

    private void Respawn()
    {
        transform.position = currentRespawnPosition;
        OnPlayerRespawn?.Invoke(); // For future use, see (declaration) explaination above
    }

    public void UpdateCheckpoint (Vector2 newSpawnPosition) // Public method for future checkpoint script
    {
        currentRespawnPosition = newSpawnPosition;
    }

    void UpdateLivesUI()
    {
        PlayerLivesText.text = currentLives.ToString();
    }

    private void GameWon()
    {
        SceneManager.LoadScene(4);
    }
}