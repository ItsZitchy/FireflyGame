using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 5f;
    public float onWall = 0;

    private Rigidbody2D rb;

    public LayerMask wallLayer;
    public Transform LWallCheck;
    public Transform RWallCheck;

    public float wallCheckRadius = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isWalled(wallLayer);
        MovementLogic();
        flyLogic();
    }

    private bool isWalled(LayerMask wallLayer)
    {
        return Physics2D.OverlapCircle(LWallCheck.position, wallCheckRadius, wallLayer)
        || Physics2D.OverlapCircle(RWallCheck.position, wallCheckRadius, wallLayer);
    }

    public void MovementLogic()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);


        if (isWalled(wallLayer))
        {
            rb.gravityScale = 0f;

            onWall = 1f;
        }
        else
        {
            rb.gravityScale = 1f;
            
            onWall = 0f;
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
