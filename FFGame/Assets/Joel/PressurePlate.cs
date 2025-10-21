using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Transform plate;
    public Vector3 pressedPosition;
    public float speed = 5f;

    [Header("Connected Doors")]
    public BoxCollider2D[] doorColliders;
    public SpriteRenderer[] doorSprites; // Add this for color changes
    public Color openColor = Color.green;
    public Color closedColor = Color.red;

    private Vector3 defaultPosition;
    private bool pressed = false;

    void Start()
    {
        defaultPosition = plate.position;

        // Set all doors to closed color initially
        foreach (var sprite in doorSprites)
            if (sprite != null)
                sprite.color = closedColor;
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
                    sprite.color = openColor; // Change color
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            pressed = false;

            foreach (var door in doorColliders)
                if (door != null)
                    door.enabled = true; // Enable collider (close)

            foreach (var sprite in doorSprites)
                if (sprite != null)
                    sprite.color = closedColor; // Change back
        }
    }
}
