using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private int count;

    private float movementX;
    private float movementY;

    public float speed = 8f;
    public float maxSpeed = 6f;     // NEW: speed cap
    public float damping = 10f;     // NEW: how quickly we stop

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);

        // Helps reduce sliding automatically
        rb.linearDamping = 2f;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0f, movementY);

        // Apply force using acceleration (mass independent)
        rb.AddForce(movement * speed, ForceMode.Acceleration);

        // Clamp horizontal velocity (prevents sliding)
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }

        // Extra damping when no input (tight control)
        if (movement.magnitude < 0.1f)
        {
            rb.linearVelocity = Vector3.Lerp(
                rb.linearVelocity,
                new Vector3(0f, rb.linearVelocity.y, 0f),
                damping * Time.fixedDeltaTime
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
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
        {
            winTextObject.SetActive(true);
        }
    }
}
