using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    public Transform player;  // Assign your player here
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        // Keep chasing the player
        agent.SetDestination(player.position);
    }

    // When enemy hits the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject); // DESTROY PLAYER
            Debug.Log("Player Destroyed");
        }
    }
}
