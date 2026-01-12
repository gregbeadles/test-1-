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

        // Face UP at start
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        UpdateDashUI();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void Update()
    {
        // Dash input (E key)
        if (Keyboard.current.eKey.wasPressedThisFrame && canDash)
        {
            StartCoroutine(Dash());
        }

        // Cooldown timer
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
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        Vector3 movement = new Vector3(movementX, 0f, movementY).normalized;
        rb.linearVelocity = movement * speed;
    }

    System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        dashCooldownTimer = dashCooldown;

        Vector3 dashDirection = new Vector3(movementX, 0f, movementY).normalized;

        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }

        rb.linearVelocity = dashDirection * dashForce;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    void UpdateDashUI()
    {
        if (canDash)
        {
            dashCooldownText.text = "Dash: READY";
        }
        else
        {
            dashCooldownText.text = "Dash: " + dashCooldownTimer.ToString("F1") + "s";
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
    }
}

