using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    // Use trigger instead of collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
