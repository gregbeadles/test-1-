using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject gameOverScreen; // Assign your Game Over panel here

    private bool gameIsOver = false;

    void Start()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false); // hide at start
    }

    // Call this function when the player dies
    public void PlayerDied()
    {
        if (gameIsOver) return; // avoid multiple calls
        gameIsOver = true;

        if (gameOverScreen != null)
        {
            // Ensure the canvas is active
            Canvas canvas = gameOverScreen.GetComponentInParent<Canvas>();
            if (canvas != null) canvas.gameObject.SetActive(true);

            gameOverScreen.SetActive(true); // show UI
        }

        Debug.Log("Game Over screen activated!");
    }
}
