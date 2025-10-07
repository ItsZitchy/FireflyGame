using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //movement
    public float moveSpeed = 4f;
    public float jumpForce = 5f;
    public float maxVerticalSpeed = 8f;

    private Rigidbody2D rb;

    
    //platform checks
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    public LayerMask wallLayer;
    public float wallCheckRadius = 0.1f;
    public Transform LWallCheck;
    public Transform RWallCheck;

    public LayerMask boxLayer;
    private Collider2D carriedBox;


    //stamina
    public float maxStamina = 100f;
    public float staminaDrainRate = 40f;
    public float staminaRegenRate = 50f;
    private float currentStamina;

    //respawn
    private Vector3 spawnPoint;

    //liftbox

    //other
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //other
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        //stamina
        currentStamina = maxStamina;

        //respawn
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
    private bool isOnBox(LayerMask boxLayer)
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,  boxLayer);
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
            jumpForce = 5f;

            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    public void Stamina()
    { 
        if (isGrounded(groundLayer) || isWalledLeft(wallLayer) || isWalledRight(wallLayer) || (isOnBox(boxLayer)))
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
            Rigidbody2D rb = carriedBox.GetComponent<Rigidbody2D>();
            carriedBox.transform.position = transform.position + new Vector3(0, -1f, 0);
            rb.angularVelocity = 0f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && carriedBox != null)
        {
            if (rb != null)
            {
                Rigidbody2D rb = carriedBox.GetComponent<Rigidbody2D>();
                rb.linearVelocity = Vector2.zero;   // stop momentum
                rb.angularVelocity = 0f;      // stop spinning just in case
            }
            carriedBox = null;
        }
    }
}
