using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;

    private float movementX;
    private float movementY;

    [Header("Movement Settings")]
    public float speed = 6f;  // Movement speed

    [Header("UI Settings")]
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private int totalPickups; // Will be calculated automatically

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        winTextObject.SetActive(false);

        // Count all pickups in the scene automatically
        totalPickups = GameObject.FindGameObjectsWithTag("PickUp").Length;

        SetCountText();

        // Prevent player from tipping over
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0f, movementY).normalized;
        rb.linearVelocity = movement * speed;

        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 0.25f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count + " / " + totalPickups;

        if (count >= totalPickups)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        winTextObject.SetActive(true);
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemies)
        {
            Rigidbody er = e.GetComponent<Rigidbody>();
            if (er != null) er.isKinematic = true;
        }

        // Optional: restart the scene automatically
        // Invoke(nameof(RestartGame), 3f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
