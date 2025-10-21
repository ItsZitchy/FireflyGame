using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Transform plate;
    public Vector3 pressedPosition;
    public float speed = 5f;

    [Header("Connected Doors")]
    public BoxCollider2D[] doorColliders;    // Colliders to enable/disable
    public SpriteRenderer[] doorSprites;     // The visuals to change
    public Sprite openSprite;                // Sprite when door is open
    public Sprite closedSprite;              // Sprite when door is closed

    private Vector3 defaultPosition;
    private bool pressed = false;

    void Start()
    {
        defaultPosition = plate.position;

        // Initialize doors with closed sprite
        foreach (var sprite in doorSprites)
            if (sprite != null)
                sprite.sprite = closedSprite;
    }

    void Update()
    {
        Vector3 target = pressed ? pressedPosition : defaultPosition;
        plate.position = Vector3.Lerp(plate.position, target, Time.deltaTime * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            pressed = true;

            foreach (var door in doorColliders)
                if (door != null)
                    door.enabled = false; // Disable collider (open)

            foreach (var sprite in doorSprites)
                if (sprite != null)
                    sprite.sprite = openSprite; // Change visual to open
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            pressed = false;

            foreach (var door in doorColliders)
                if (door != null)
                    door.enabled = true; // Enable collider (closed)

            foreach (var sprite in doorSprites)
                if (sprite != null)
                    sprite.sprite = closedSprite; // Change visual to closed
        }
    }
}
