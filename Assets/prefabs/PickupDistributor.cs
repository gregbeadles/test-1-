using UnityEngine;

public class PickupDistributor : MonoBehaviour
{
    public GameObject pickupPrefab;      // The pickup cube prefab
    public int numberOfPickups = 20;     // How many to place
    public Vector3 areaMin;              // Lower bounds of spawn area
    public Vector3 areaMax;              // Upper bounds of spawn area
    public float minDistance = 1.5f;     // Minimum distance between pickups

    private void Start()
    {
        PlacePickups();
    }
   
    void PlacePickups()
    {
        int placed = 0;
        int attempts = 0;
        Vector3[] positions = new Vector3[numberOfPickups];

        while (placed < numberOfPickups && attempts < numberOfPickups * 10)
        {
            attempts++;
            Vector3 candidate = new Vector3(
                Random.Range(areaMin.x, areaMax.x),
                Random.Range(areaMin.y, areaMax.y),
                Random.Range(areaMin.z, areaMax.z)
            );

            bool tooClose = false;
            for (int i = 0; i < placed; i++)
            {
                if (Vector3.Distance(candidate, positions[i]) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                positions[placed] = candidate;
                Instantiate(pickupPrefab, candidate, Quaternion.identity);
                placed++;
            }
        }

        Debug.Log($"Placed {placed} pickups with {attempts} attempts.");
    }
}
