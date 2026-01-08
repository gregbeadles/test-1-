using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyChase : MonoBehaviour
{
    public Transform player;      // Player to chase
    public float speed = 3f;      // Enemy movement speed

    void Update()
    {
        if (player == null) return;

        // Move toward the player
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        // Optional: rotate to face player
        transform.LookAt(player);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");

        // Reload the scene (simple game over)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
