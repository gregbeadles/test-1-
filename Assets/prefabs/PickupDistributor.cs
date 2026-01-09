using UnityEngine;
using UnityEngine.AI;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;   // Assign your pickup prefab in the inspector
    public float spawnRadius = 10f;   // Radius around the spawn point to find a valid position
    public int numberOfPickups = 5;   // Number of pickups to spawn
    public int maxTries = 10;         // Maximum number of attempts to find a valid spawn point

    private void Start()
    {
        SpawnPickups(numberOfPickups); // Spawn the pickups at start
    }

    // Method to spawn a specific number of pickups
    void SpawnPickups(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Try to spawn each pickup
            Vector3 spawnPosition = FindValidSpawnPosition(transform.position, spawnRadius);

            if (spawnPosition != Vector3.zero)
            {
                // Instantiate the pickup at the valid position
                Instantiate(pickupPrefab, spawnPosition, Quaternion.identity);
                Debug.Log("Pickup Spawned at: " + spawnPosition);
            }
            else
            {
                Debug.LogWarning("Failed to find a valid spawn position.");
            }
        }
    }

    // Try to find a valid spawn position within a given radius
    Vector3 FindValidSpawnPosition(Vector3 origin, float radius)
    {
        Vector3 randomPosition = origin;

        // Try up to 'maxTries' times to find a valid position
        for (int i = 0; i < maxTries; i++)
        {
            // Random position within the radius
            randomPosition = origin + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));

            // Check if the position is valid on the NavMesh (i.e., on open ground)
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                return hit.position; // Return the valid position
            }
        }

        // Return Vector3.zero if no valid position was found after maxTries
        return Vector3.zero;
    }
}
