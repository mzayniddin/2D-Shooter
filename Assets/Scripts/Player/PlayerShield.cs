using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public GameObject shieldObject;  // Reference to the shield visual
    public float shieldDuration = 5f;  // Duration for how long the shield stays active
    private Collider2D shieldCollider;  // Reference to the shield's collider
    public bool shieldIsActive = true;

    void Start()
    {
        // Get the Collider2D component of the shieldObject
        shieldCollider = shieldObject.GetComponent<Collider2D>();

        // Ensure the shield collider is initially disabled
        shieldCollider.enabled = false;
        shieldObject.SetActive(false);  // Make sure shield is initially invisible
    }

    // Function to activate the shield
    public void ActivateShield()
    {
        if (shieldObject != null && shieldCollider != null)
        {
            shieldObject.SetActive(true);  // Show the shield visual
            shieldCollider.enabled = true;  // Enable the shield collider to block projectiles

            // Automatically disable the shield after shieldDuration
            Invoke("DisableShield", shieldDuration);
        }
    }

    // Function to deactivate the shield
    public void DisableShield()
    {
        if (shieldObject != null && shieldCollider != null)
        {
            shieldObject.SetActive(false);  // Hide the shield visual
            shieldCollider.enabled = false;  // Disable the shield collider
        }
    }

    // Detect collision with the shield collider and enemy projectiles
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collects a shield bubble power-up
        if (collision.CompareTag("ShieldBubble"))
        {
            ActivateShield();  // Activate the shield when power-up is collected
            Destroy(collision.gameObject);  // Destroy the power-up object
        }
    }

    // Detect collision with projectiles via the shield collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            // If the shield is active, destroy the projectile
            Destroy(collision.gameObject);

            // Optional: you can add effects when the shield blocks a projectile here
            Debug.Log("Projectile blocked by shield.");
        }
    }
}
