using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;

    private float movementX;
    private float movementY;

    [Header("Movement Settings")]
    public float speed = 6f;

    [Header("UI Settings")]
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private int totalPickups;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        winTextObject.SetActive(false);

        totalPickups = GameObject.FindGameObjectsWithTag("PickUp").Length;
        SetCountText();

        // FIX: rotate player to face UP at start
        transform.rotation = Quaternion.Euler(90f, 0f, 0f); // try -90 if needed
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
    }
}
