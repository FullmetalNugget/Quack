using UnityEngine;
using UnityEngine.InputSystem;

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
    public float stopSpeed = 2f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;


    private bool isRunning;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isSliding;
    private float slideTimer;
    private bool canDoubleJump;

    private Vector3 wishDir;
    private bool jumpPressed;
    private bool slidePressed;
    private Animator anim;

    public string adUnitId = "Interstitial_Android"; // from Unity Dashboard
    public string gameId = "5988341";

#if UNITY_IOS
    public string gameId = "5988340";
    public string adUnitId = "Interstitial_IOS"; // from Unity Dashboard
#endif

    private PlayerControls controls;
    private Vector2 moveInput;


    void Awake()
    {
        controls = new PlayerControls();

        // Movement
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled  += ctx => moveInput = Vector2.zero;

        // Sprint
        controls.Player.Sprint.performed += ctx => isRunning = true;
        controls.Player.Sprint.canceled  += ctx => isRunning = false;

        // Dash
        controls.Player.Dash.performed += ctx => slidePressed = true;

        //Jump
        controls.Player.Jump.performed += ctx => jumpPressed = true;

    }

    void OnEnable()  => controls.Enable();
    void OnDisable() => controls.Disable();


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezePositionZ;
        animController = GetComponent<Animator>();  // Get the Animator component attached to this GameObject
    }

    private void Update()
    {
        // Capture input
        wishDir = new Vector3(moveInput.x, 0f, 0f);
        animController();
        RotateCharacter(moveInput.x);
    }

    private void FixedUpdate()
    {
        float curTime = Time.time;
        isGrounded = CheckGrounded();

        // Reset double jump if grounded
        if (isGrounded)
            canDoubleJump = true;

        HandleSliding(curTime);

        Vector3 velocity = rb.linearVelocity;
        Vector3 moveDir = wishDir.sqrMagnitude > 0.001f ? wishDir.normalized : Vector3.zero;
        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        if (isGrounded)
        {
            if (!isSliding)
            {
                ApplyFriction(ref velocity, friction, stopSpeed);
                GroundAccelerate(ref velocity, moveDir, targetSpeed, accelSpeed);
            }
        }
        else
        {
            AirAccelerate(ref velocity, moveDir, Mathf.Min(targetSpeed, maxAirSpeed), accelSpeed * airControl);
        }

        rb.linearVelocity = velocity;

        HandleJump();
    }

    private void HandleSliding(float curTime)
    {
        if (!isSliding)
        {
            if (slidePressed && curTime - slideTimer > slideCooldown)
            {
                StartSlide(curTime);
                animController.Play("Slide");
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
        rb.velocity = new Vector3(rb.velocity.x > 0 ? slideSpeed : -slideSpeed, 0f, 0f);
    }

    private void GroundAccelerate(ref Vector3 velocity, Vector3 wishDir, float wishSpeed, float accelerate)
    {
        if (wishDir.sqrMagnitude < 0.0001f)
            return;

        // Project current velocity along desired direction
        float currentSpeed = Vector3.Dot(new Vector3(velocity.x, 0f, 0f), wishDir);
        float addSpeed = Mathf.Max(wishSpeed - currentSpeed, 0);

        if (addSpeed <= 0)
            return;

        float accelThisFrame = Mathf.Min(accelerate * Time.fixedDeltaTime * wishSpeed, addSpeed);
        velocity += accelThisFrame * wishDir;
    }

    private void AirAccelerate(ref Vector3 velocity, Vector3 wishDir, float wishSpeed, float accelerate)
    {
        if (wishDir.sqrMagnitude < 0.0001f)
            return;

        float currentSpeed = Vector3.Dot(new Vector3(velocity.x, 0f, 0f), wishDir);
        float addSpeed = Mathf.Max(wishSpeed - currentSpeed, 0);

        if (addSpeed <= 0)
            return;

        float accelThisFrame = accelerate * wishSpeed * Time.fixedDeltaTime;
        accelThisFrame = Mathf.Min(accelThisFrame, addSpeed);
        velocity += accelThisFrame * wishDir;
    }

    private void ApplyFriction(ref Vector3 velocity, float friction, float stopSpeed)
    {
        Vector3 planarVelocity = new Vector3(velocity.x, 0f, velocity.z);
        float speed = planarVelocity.magnitude;

        if (speed < 0.001f) return;

        float control = Mathf.Max(speed, stopSpeed);
        float drop = control * friction * Time.fixedDeltaTime;
        float newSpeed = Mathf.Max(speed - drop, 0f);

        if (newSpeed != speed)
        {
            float scale = newSpeed / speed;
            velocity.x *= scale;
            velocity.z *= scale;
        }
    }

    private void HandleJump()
    {
        if (!jumpPressed) return;

        if (isGrounded)
        {
            Jump();
            animController.Play("Jump");
        }
        else if (canDoubleJump)
        {
            Jump();
            animController.Play("Jump");
            canDoubleJump = false;
        }

        jumpPressed = false;
    }

    private void Jump()
    {
        Vector3 velocity = rb.velocity;
        velocity.y = jumpForce;
        rb.velocity = velocity;
    }

    private bool CheckGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void RotateCharacter(float horizontal)
    {
        if (horizontal > 0.01f)
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        else if (horizontal < -0.01f)
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }


    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void UpdateAnimator()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x), 0.1f, Time.deltaTime);
    }
}
