using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;  // Required for scene management

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    public Transform player;  // Assign your player here
    private NavMeshAgent agent;

    [Header("Enemy Settings")]
    public float speed = 6f;            // Movement speed
    public float acceleration = 50f;    // How fast enemy reaches full speed
    public float angularSpeed = 1200f;  // How fast enemy turns to face player

    [Header("Game Over UI")]
    public GameObject gameOverScreen;  // Assign your Game Over screen UI panel here

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Apply sharp movement settings
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.angularSpeed = angularSpeed;
        agent.stoppingDistance = 0f;   // Enemy reaches player exactly
        agent.updateRotation = true;   // Let NavMeshAgent handle rotation

        // Hide the Game Over screen initially
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
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
            ShowGameOverScreen();  // show UI first
            Destroy(other.gameObject); // then destroy player
            Debug.Log("Player Destroyed");
        }
    }


    // Show Game Over screen
    void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);  // Activate the Game Over UI
        }
    }
}
