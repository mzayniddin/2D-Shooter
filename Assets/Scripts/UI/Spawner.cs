using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject shieldBubblePrefab;  // Reference to the ShieldBubble prefab
    public float spawnInterval = 10f;  // Time interval between spawns
    public Vector2 spawnAreaMin;  // Minimum X and Y bounds for random spawn area
    public Vector2 spawnAreaMax;  // Maximum X and Y bounds for random spawn area

    private float timeSinceLastSpawn = 0f;

    void Update()
    {
        // Increment the time since the last spawn
        timeSinceLastSpawn += Time.deltaTime;

        // Check if it's time to spawn a new ShieldBubble
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnShieldBubble();
            timeSinceLastSpawn = 0f;  // Reset the timer
        }
    }

    // Function to spawn the ShieldBubble at a random position
    void SpawnShieldBubble()
    {
        // Generate a random position within the defined area
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        // Instantiate the ShieldBubble at the random position
        Instantiate(shieldBubblePrefab, spawnPosition, Quaternion.identity);
    }
}
