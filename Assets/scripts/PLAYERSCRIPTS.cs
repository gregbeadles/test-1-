using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;

    private float movementX;
    private float movementY;

    public float speed = 6f;  // Movement speed
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);

        // Prevent the player from tipping over
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
        // Create a movement vector
        Vector3 movement = new Vector3(movementX, 0f, movementY).normalized;

        // Directly set the velocity (sharp, responsive movement)
        rb.linearVelocity = movement * speed;

        // Optional: rotate the player to face movement direction
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
        countText.text = "Count: " + count;

        if (count >= 12)
            winTextObject.SetActive(true);
    }
}
