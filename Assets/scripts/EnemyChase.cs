using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    public Transform player;  // Assign your player in the inspector
    private NavMeshAgent agent;

    [Header("Enemy Settings")]
    public float speed = 6f;            // Movement speed
    public float acceleration = 50f;    // How fast enemy reaches full speed
    public float angularSpeed = 1200f;  // How fast enemy turns to face player

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Apply sharp movement settings
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.angularSpeed = angularSpeed;
        agent.stoppingDistance = 0f;   // Enemy reaches player exactly
        agent.updateRotation = true;   // Let NavMeshAgent handle rotation
    }

    void Update()
    {
        if (player == null) return;

        // Constantly chase the player
        agent.SetDestination(player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject); // Destroy player
            Debug.Log("Player Destroyed");
        }
    }
}

