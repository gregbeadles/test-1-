using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    [Header("Enemy Settings")]
    public float speed = 6f;
    public float acceleration = 50f;
    public float angularSpeed = 1200f;

    [Header("Game Over UI")]
    public GameObject gameOverScreen;

    private bool canChase = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.angularSpeed = angularSpeed;
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    void Update()
    {
        if (!canChase || player == null)
            return;

        agent.SetDestination(player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canChase)
            return;

        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Debug.Log("Player Destroyed");

            ShowGameOverScreen();
        }
    }

    // Called when all pickups are collected
    public void StopChasing()
    {
        canChase = false;
        agent.speed = 0;
        agent.isStopped = true;
        agent.ResetPath();
    }

    void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }
}
