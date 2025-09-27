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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded(groundLayer);
        isWalled(wallLayer);
        MovementLogic();
        flyLogic();
    }

    private bool isGrounded(LayerMask groundLayer)
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    private bool isWalled(LayerMask wallLayer)
    {
        return Physics2D.OverlapCircle(LWallCheck.position, wallCheckRadius, wallLayer)
        || Physics2D.OverlapCircle(RWallCheck.position, wallCheckRadius, wallLayer);
    }

    public void MovementLogic()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (isWalled(wallLayer)) 
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

        if (isGrounded(groundLayer) || isWalled(wallLayer))
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
}
