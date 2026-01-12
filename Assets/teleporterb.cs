using UnityEngine;

public class teleporterb : MonoBehaviour
{
    public Transform destination;
    public Vector3 exitOffset;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Teleport player using transform
        other.transform.position = destination.position + exitOffset;

        // Stop any Rigidbody motion if player has Rigidbody
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Optional: ensure physics update
        Physics.SyncTransforms();
    }
}
