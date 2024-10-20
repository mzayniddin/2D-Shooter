using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    public float speedBoostMultiplier = 2f;  // How much to multiply the player's speed
    public float boostDuration = 5f;  // How long the speed boost lasts

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collided with the SpeedBooster
        Controller playerController = collision.GetComponent<Controller>();  // Use the correct class name: Controller
        if (playerController != null)
        {
            // Activate speed boost on the player
            playerController.ActivateSpeedBoost(speedBoostMultiplier, boostDuration);

            // Destroy the SpeedBooster bubble after it's collected
            Destroy(gameObject);
        }
    }
}
