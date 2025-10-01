using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 3f;
    public float maxVerticalSpeed = 8f;

    private Rigidbody2D rb;


    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    public LayerMask wallLayer;
    public float wallCheckRadius = 0.1f;
    public Transform LWallCheck;
    public Transform RWallCheck;

    public LayerMask boxLayer;
    private Collider2D carriedBox;


    public float maxStamina = 100f;
    public float staminaDrainRate = 40f;
    public float staminaRegenRate = 50f;
    private float currentStamina;

    private Vector3 spawnPoint;

    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentStamina = maxStamina;

        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded(groundLayer);
        isWalledLeft(wallLayer);
        isWalledRight(wallLayer);
        MovementLogic();
        Stamina();
        FlyLogic();
        SpriteFlip();
        LiftBox();
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
    private bool IsBoxGrounded(Collider2D box)
    {
        if (box == null) return false;

        // Check just below the box
        float extraHeight = 0.05f;
        RaycastHit2D hit = Physics2D.Raycast(box.bounds.center, Vector2.down, box.bounds.extents.y + extraHeight, groundLayer);

        return hit.collider != null;
    }

    private Collider2D GetBoxUnderPlayer(LayerMask boxLayer)
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, boxLayer);
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
            jumpForce = 3f;

            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    public void Stamina()
    {
        Collider2D box = GetBoxUnderPlayer(boxLayer);

        if (isGrounded(groundLayer) || isWalledLeft(wallLayer) || isWalledRight(wallLayer) || (box != null && IsBoxGrounded(box)))
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
        else
        {
            currentStamina -= (staminaDrainRate / 2) * Time.deltaTime;
        }
    }

    public void FlyLogic()
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

    public void SpriteFlip()
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

    public void Respawn()
    {
        transform.position = spawnPoint;
        Debug.Log("Player respawned!");
    }

    public void LiftBox()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Try to grab a box if standing on one
            Collider2D box = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, boxLayer);
            if (box != null)
            {
                carriedBox = box;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && carriedBox != null)
        {
            carriedBox.transform.position = transform.position + new Vector3(0, -1f, 0);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && carriedBox != null)
        {
            Rigidbody2D rb = carriedBox.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;   // stop momentum
                rb.angularVelocity = 0f;      // stop spinning just in case
            }
            carriedBox = null;
        }
    }
}
