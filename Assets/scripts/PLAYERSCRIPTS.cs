using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;

    private float movementX;
    private float movementY;
    private Vector3 lastMoveDirection = Vector3.forward;

    [Header("Movement Settings")]
    public float speed = 6f;

    [Header("Dash Settings")]
    public float dashForce = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 6f;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    [Header("Jump Settings")]
    public float jumpHeight = 6f;       // Max jump height
    public float jumpCooldown = 10f;    // Jump cooldown in seconds
    private bool canJump = true;
    private float jumpCooldownTimer = 0f;

    [Header("UI Settings")]
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public TextMeshProUGUI dashCooldownText;
    public TextMeshProUGUI jumpCooldownText;

    private int totalPickups;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        count = 0;

        winTextObject.SetActive(false);
        totalPickups = GameObject.FindGameObjectsWithTag("PickUp").Length;
        SetCountText();

        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        UpdateDashUI();
        UpdateJumpUI();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 input = movementValue.Get<Vector2>();
        if (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
        {
            float x = 0f;
            float y = 0f;
            if (Keyboard.current.wKey.isPressed) y += 1f;
            if (Keyboard.current.sKey.isPressed) y -= 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;

            movementX = x;
            movementY = y;
        }
        else
        {
            movementX = input.x;
            movementY = input.y;
        }
    }

    void Update()
    {
        // Dash
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && canDash)
            StartCoroutine(Dash());

        // Jump (can jump even in mid-air if cooldown is over)
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame && canJump)
            Jump();

        // Dash cooldown timer
        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0f)
            {
                canDash = true;
                dashCooldownTimer = 0f;
            }
            UpdateDashUI();
        }

        // Jump cooldown timer
        if (!canJump)
        {
            jumpCooldownTimer -= Time.deltaTime;
            if (jumpCooldownTimer <= 0f)
            {
                canJump = true; // Re-enable jump once cooldown ends
                jumpCooldownTimer = 0f;
            }
            UpdateJumpUI();
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Vector3 horizontalMovement = new Vector3(movementX, 0f, movementY).normalized * speed;
            Vector3 velocity = rb.linearVelocity;
            velocity.x = horizontalMovement.x;
            velocity.z = horizontalMovement.z;
            rb.linearVelocity = velocity;

            if (horizontalMovement.sqrMagnitude > 0.01f)
                lastMoveDirection = horizontalMovement.normalized;
        }
    }

    void Jump()
    {
        // Calculate velocity required to reach jumpHeight
        float jumpVelocity = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics.gravity.y));
        Vector3 v = rb.linearVelocity;
        v.y = jumpVelocity;
        rb.linearVelocity = v;

        canJump = false; // Trigger cooldown
        jumpCooldownTimer = jumpCooldown;
        UpdateJumpUI();
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        dashCooldownTimer = dashCooldown;

        Vector3 inputDir = new Vector3(movementX, 0f, movementY);
        Vector3 dashDir = inputDir.sqrMagnitude > 0.01f ? inputDir.normalized : lastMoveDirection;

        rb.linearVelocity = dashDir * dashForce + new Vector3(0, rb.linearVelocity.y, 0);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    void UpdateDashUI()
    {
        if (dashCooldownText == null) return;
        dashCooldownText.text = canDash ? "Dash: READY" : $"Dash: {dashCooldownTimer:F1}s";
    }

    void UpdateJumpUI()
    {
        if (jumpCooldownText == null) return;
        jumpCooldownText.text = canJump ? "Jump: READY" : $"Jump: {jumpCooldownTimer:F1}s";
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
        if (countText != null)
            countText.text = $"Count: {count} / {totalPickups}";

        if (count >= totalPickups && winTextObject != null)
            winTextObject.SetActive(true);
    }

    public int GetPickupCount() => count;
}
