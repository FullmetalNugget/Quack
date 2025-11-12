using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 10f;
    public float runSpeed = 25f;
    public float jumpForce = 7f;
    public float slideSpeed = 14f;
    public float slideDuration = 0.5f;
    public float slideCooldown = 1.0f;

    [Header("Quake Movement")]
    public float accelSpeed = 4f;
    public float maxAirSpeed = 5f;
    public float friction = 2f;
    public float airControl = 0.2f;
    public float flipStrength = -0.7f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isSliding;
    private float slideTimer;
    private bool canDoubleJump;

    private Vector3 wishDir;
    private bool isRunning;
    private bool jumpPressed;
    private bool slidePressed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezePositionZ;
    }

    private void Update()
    {
        // Capture input
        wishDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            slidePressed = true;
    }

    private void FixedUpdate()
    {
        float curTime = Time.time;
        isGrounded = CheckGrounded();

        // Reset double jump if grounded
        if (isGrounded)
            canDoubleJump = true;

        HandleSliding(curTime);

        if (isGrounded)
        {
            if (!isSliding)
                ApplyFriction(rb, friction);

            Accelerate(rb, wishDir, isRunning ? runSpeed : walkSpeed, accelSpeed);
        }
        else
        {
            Accelerate(rb, wishDir, maxAirSpeed, accelSpeed * airControl);
        }

        HandleJump();
    }

    private void HandleSliding(float curTime)
    {
        if (!isSliding)
        {
            if (slidePressed && curTime - slideTimer > slideCooldown)
            {
                StartSlide(curTime);
            }
        }
        else if (curTime - slideTimer > slideDuration)
        {
            isSliding = false;
        }

        slidePressed = false; // reset input
    }

    private void StartSlide(float curTime)
    {
        isSliding = true;
        slideTimer = curTime;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x > 0 ? slideSpeed : -slideSpeed, 0f, 0f);
    }

    private void Accelerate(Rigidbody rb, Vector3 wishDir, float wishSpeed, float accelerate)
    {
        Vector3 velocity = rb.linearVelocity;

        // Project current velocity along desired direction
        float currentSpeed = Vector3.Dot(new Vector3(velocity.x, 0, 0), wishDir);
        float addSpeed = Mathf.Max(wishSpeed - currentSpeed, 0);

        if (addSpeed <= 0)
            return;

        float accelThisFrame = Mathf.Min(accelerate * Time.fixedDeltaTime * wishSpeed, addSpeed);
        velocity += accelThisFrame * wishDir;

        rb.linearVelocity = velocity;
    }

    private void ApplyFriction(Rigidbody rb, float friction)
    {
        Vector3 velocity = rb.linearVelocity;
        float speed = velocity.magnitude;

        if (speed < 0.001f) return;

        float drop = speed * friction * Time.fixedDeltaTime;
        rb.linearVelocity = velocity * Mathf.Max((speed - drop) / speed, 0f);
    }

    private void HandleJump()
    {
        if (!jumpPressed) return;

        if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            Jump();
            canDoubleJump = false;
        }

        jumpPressed = false;
    }

    private void Jump()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;
    }

    private bool CheckGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}