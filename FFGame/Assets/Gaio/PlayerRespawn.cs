using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 currentCheckpoint;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentCheckpoint = transform.position;
    }

    public void SetCheckpoint(Vector2 newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }
}
