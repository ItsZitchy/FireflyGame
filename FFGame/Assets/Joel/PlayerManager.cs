using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 5f;

    private Rigidbody2D rb;

    public LayerMask wallLayer;
    public Transform wallCheck;

    public float wallCheckRadius = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementLogic();
        flyLogic();
    }

    private bool isWalled(LayerMask wallLayer)
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
    }

    public void MovementLogic()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);


        if (isWalled(wallLayer))
        {
            rb.gravityScale = 0f;  // Turn off gravity
        }
        else
        {
            rb.gravityScale = 1f;  // Normal gravity
        }
    }

    public void flyLogic()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }
    }
}
