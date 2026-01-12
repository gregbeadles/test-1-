using UnityEngine;

public class SpinTeleporter : MonoBehaviour
{
    [Header("Spin Settings")]
    public Vector3 rotationSpeed = new Vector3(0f, 180f, 0f); // degrees per second

    void Update()
    {
        // Rotate around local axes
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
