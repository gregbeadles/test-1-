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

    [Header("UI Settings")]
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public TextMeshProUGUI dashCooldownText;

    private int totalPickups;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        winTextObject.SetActive(false);

        totalPickups = GameObject.FindGameObjectsWithTag("PickUp").Length;
        SetCountText();

        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        UpdateDashUI();
    }

    // OnMove now filters out arrow keys while keeping WASD & gamepad
    void OnMove(InputValue movementValue)
    {
        Vector2 input = movementValue.Get<Vector2>();

        // Detect if input comes from keyboard
        if (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
        {
            float x = 0f;
            float y = 0f;

            // Only allow WASD keys
            if (Keyboard.current.wKey.isPressed) y += 1f;
            if (Keyboard.current.sKey.isPressed) y -= 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;

            movementX = x;
            movementY = y;

            // Ignore arrow keys completely
            // (do not include Keyboard.current.upArrowKey, etc.)
        }
        else
        {
            // Gamepad or other devices: use the input vector directly
            movementX = input.x;
            movementY = input.y;
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && canDash)
            StartCoroutine(Dash());

        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0f)
            {
                canDash = true;
                dashCooldownTimer = 0f;
            }
        }

        UpdateDashUI();
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        Vector3 movement = new Vector3(movementX, 0f, movementY);

        if (movement.sqrMagnitude > 0.01f)
            lastMoveDirection = movement.normalized;

        rb.linearVelocity = movement.normalized * speed;
    }

    System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        dashCooldownTimer = dashCooldown;

        Vector3 inputDir = new Vector3(movementX, 0f, movementY);
        Vector3 dashDir = inputDir.sqrMagnitude > 0.01f ? inputDir.normalized : lastMoveDirection;

        rb.linearVelocity = dashDir * dashForce;

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    void UpdateDashUI()
    {
        if (dashCooldownText == null) return;

        dashCooldownText.text = canDash ? "Dash: READY" : $"Dash: {dashCooldownTimer:F1}s";
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
        countText.text = $"Count: {count} / {totalPickups}";
        if (count >= totalPickups) WinGame();
    }

    void WinGame()
    {
        winTextObject.SetActive(true);
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        foreach (var enemy in FindObjectsOfType<EnemyChase>())
            enemy.StopChasing();
    }
}
