using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLocomotion : MonoBehaviour
{
    private PlayerManager playerManager;
    private AnimatorManager animatorManager;
    private InputManager inputManager;
    private Transform cameraObject;
    private Vector3 moveDirection;
    private Vector3 targetDirection;
    private Rigidbody playerRigidbody;

    [Header("Falling")] 
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public LayerMask groundLayer;
    public float rayCastHeightOffset = 0.5f;

    [Header("Movement Flags")] 
    public bool isGrounded;
    public bool isJumping;
    public bool isJumpLag = false;

    [Header("Movement Speeds")]
    [SerializeField] public float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 20f;

    [Header("Jump Speeds")] 
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float gravityIntensity = -20f;
    [SerializeField] private float jumpLag = 0.2f;

    [Header("UI Timer")]
    public Text timerText;
    private float countdownTime = 60f;

    [Header("Attempts System")]
    public int maxAttempts = 3;
    private int remainingAttempts;
    private Vector3 startPosition;

    // Invisible Player Flag
    private bool isInvisible = false;
    private Renderer playerRenderer;
    private Collider playerCollider;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        if (Camera.main is { }) cameraObject = Camera.main.transform;

        // Get the Renderer and Collider components
        playerRenderer = GetComponent<Renderer>();
        playerCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        startPosition = transform.position; // Simpan posisi awal
        remainingAttempts = maxAttempts;   // Set jumlah percobaan
        StartCoroutine(TimerCountdown());
    }

    private void Update()
    {
        // Deteksi tap di layar setelah jatuh
        switch (isGrounded)
        {
            case false:
                if (Input.GetMouseButtonDown(0))
                {
                    ResetToStartPosition();
                }
                break;
            default: break;
        }

        // Toggle invisibility when pressing the 'I' key
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInvisibility();
        }
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        if (playerManager.isInteracting || isJumpLag) return;
        HandleMovement();
        HandleRotation();
    }

    public void HandleJumping()
    {
        switch (isGrounded)
        {
            case true:
                StartCoroutine(JumpCoroutine());
                animatorManager.animator.SetBool("isJumping", true);
                animatorManager.PlayTargetAnimation("Jump", false);
                break;
            default: break;
        }
    }

    private IEnumerator JumpCoroutine()
    {
        isJumpLag = true;
        playerRigidbody.linearVelocity = new Vector3(0, playerRigidbody.linearVelocity.y);
        yield return new WaitForSecondsRealtime(jumpLag);
        isJumpLag = false;
        float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
        playerRigidbody.AddForce(new Vector3(0, jumpingVelocity, 0), ForceMode.Impulse);
    }

    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput; 
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        var movementVelocity = moveDirection * movementSpeed;
        movementVelocity.y = playerRigidbody.linearVelocity.y;
        playerRigidbody.linearVelocity = movementVelocity;
    }

    private void HandleRotation()
    {
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0; 
        
        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        var targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y += rayCastHeightOffset;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer += Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, 1f, groundLayer))
        {
            if (!isGrounded && playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private IEnumerator TimerCountdown()
    {
        while (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }

        countdownTime = 0;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int hours = Mathf.FloorToInt(countdownTime / 3600);  // Calculate hours
        int minutes = Mathf.FloorToInt((countdownTime % 3600) / 60);  // Calculate minutes
        int seconds = Mathf.FloorToInt(countdownTime % 60);  // Calculate seconds

        timerText.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";  // Format as HH:MM:SS
    }

    private void ResetToStartPosition()
    {
        if (remainingAttempts > 0)
        {
            transform.position = startPosition; // Reset posisi
            countdownTime = 60f;               // Reset timer
            UpdateTimerUI();
            remainingAttempts--;               // Kurangi sisa attempts
        }
        else
        {
            Debug.Log("Out of attempts!");
            // Tambahkan logika jika attempts habis (game over atau lainnya)
        }
    }

    // Method to toggle invisibility
    private void ToggleInvisibility()
    {
        isInvisible = !isInvisible;
        playerRenderer.enabled = !isInvisible;  // Hide or show player mesh
        playerCollider.enabled = !isInvisible; // Disable or enable collisions
    }
}
