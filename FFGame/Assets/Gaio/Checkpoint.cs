using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite activatedSprite;
    private bool activated = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!activated)
            {
                activated = true;
                spriteRenderer.sprite = activatedSprite;


                other.GetComponent<PlayerRespawn>().SetCheckpoint(transform.position);
            }
        }
    }
}
