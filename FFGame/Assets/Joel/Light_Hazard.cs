using UnityEngine;
using System.Collections;

public class Light_Hazard : MonoBehaviour
{
    public float activeTime = 2f;   // time the light is ON
    public float inactiveTime = 2f; // time the light is OFF
    private bool isActive = true;   // current state

    private Collider2D col;
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>(); // optional, if you want to show on/off visually
        StartCoroutine(ToggleLight());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator ToggleLight()
    {
        while (true)
        {
            isActive = !isActive;
            col.enabled = isActive; // enable/disable collider
            if (sr != null) sr.enabled = isActive; // show/hide sprite
            yield return new WaitForSeconds(isActive ? activeTime : inactiveTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                player.Respawn();
            }
        }
    }
}
