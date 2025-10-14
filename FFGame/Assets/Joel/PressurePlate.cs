using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Transform plate;          // The part that moves
    public Vector3 pressedPosition;  // Lower position when pressed
    public float speed = 5f;         // How fast it moves

    private Vector3 defaultPosition; // Original position
    private bool pressed = false;    // Is something on it?

    void Start()
    {
        defaultPosition = plate.position;
    }

    void Update()
    {
        // Move plate smoothly up or down
        Vector3 target = pressed ? pressedPosition : defaultPosition;
        plate.position = Vector3.Lerp(plate.position, target, Time.deltaTime * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Press when player or box steps on it
        if (other.CompareTag("Player") || other.CompareTag("Box"))
            pressed = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Release when player or box leaves
        if (other.CompareTag("Player") || other.CompareTag("Box"))
            pressed = false;
    }
}
