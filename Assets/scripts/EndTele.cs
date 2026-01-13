using UnityEngine;

public class EndTele : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject objectToSpawn;    // The object that will appear after 40 pickups
    public Transform spawnLocation;     // Where the object will appear

    [Header("Player Settings")]
    public PlayerController playerController; // Reference to your PlayerController script
    public int requiredPickups = 40;         // Number of pickups needed to spawn

    private bool hasSpawned = false;

    void Update()
    {
        // Already spawned? Do nothing
        if (hasSpawned) return;

        // Check if player has collected enough pickups
        if (playerController != null && playerController.GetPickupCount() >= requiredPickups)
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn != null && spawnLocation != null)
        {
            Instantiate(objectToSpawn, spawnLocation.position, spawnLocation.rotation);
            hasSpawned = true;
        }
    }
}
