using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 10f;
    public float maxVerticalSpeed = 10f;

    private Rigidbody2D rb;


    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    public LayerMask wallLayer;
    public float wallCheckRadius = 0.1f;
    public Transform LWallCheck;
    public Transform RWallCheck;


    public float maxStamina = 100f;
    public float staminaDrainRate = 40f;
    public float staminaRegenRate = 50f;
    private float currentStamina;

    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded(groundLayer);
        isWalledLeft(wallLayer);
        isWalledRight(wallLayer);
        MovementLogic();
        flyLogic();
        spriteFlip();
    }

    private bool isGrounded(LayerMask groundLayer)
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    private bool isWalledLeft(LayerMask wallLayer)
    {
        return Physics2D.OverlapCircle(LWallCheck.position, wallCheckRadius, wallLayer);
    }
    private bool isWalledRight(LayerMask wallLayer)
    {
        return Physics2D.OverlapCircle(RWallCheck.position, wallCheckRadius, wallLayer);
    }

    public void MovementLogic()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (isWalledLeft(wallLayer) || isWalledRight(wallLayer))
        {
            rb.gravityScale = 0f;
            jumpForce = 0f;

            rb.linearVelocity = new Vector2(moveInput * moveSpeed, verticalInput * moveSpeed);
        }
        else
        {
            rb.gravityScale = 1f;
            jumpForce = 10f;

            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        if (isGrounded(groundLayer) || isWalledLeft(wallLayer) || isWalledRight(wallLayer))
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
        else
        {
            currentStamina -= (staminaDrainRate /2) * Time.deltaTime;
        }
    }

    public void flyLogic()
    {
        if (Input.GetButton("Jump") && currentStamina > 0f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            currentStamina -= staminaDrainRate * Time.deltaTime;
        }

        if (rb.linearVelocity.y > maxVerticalSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxVerticalSpeed);
        }
    }

    public void spriteFlip()
    {
        if (!isWalledLeft(wallLayer) && !isWalledRight(wallLayer))
        {
            spriteRenderer.flipY = false;
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                animator.SetBool("OnWall", false);
                spriteRenderer.flipX = false;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                animator.SetBool("OnWall", false);
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            animator.SetBool("OnWall", true);

            if (Input.GetAxisRaw("Vertical") > 0)
            {
                spriteRenderer.flipY = false;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                spriteRenderer.flipY = true;
            }
        }
    }
}
