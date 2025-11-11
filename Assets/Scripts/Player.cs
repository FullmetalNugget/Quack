using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float jumpForce = 7f;
    public float slideSpeed = 14f;
    public float slideDuration = 0.5f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isSliding;
    private float slideTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezePositionZ;
    }

    void Update()
    {
        CheckGrounded();

        float moveInput = Input.GetAxisRaw("Horizontal");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (!isSliding)
        {
            Vector3 move = new Vector3(moveInput * currentSpeed, rb.linearVelocity.y, 0f);
            rb.linearVelocity = move;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && Mathf.Abs(moveInput) > 0.1f)
        {
            StartSlide(moveInput);
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
                StopSlide();
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded && rb.linearVelocity.y <= 0)
            canDoubleJump = true;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
    }

    void StartSlide(float moveInput)
    {
        isSliding = true;
        slideTimer = slideDuration;
        rb.linearVelocity = new Vector3(moveInput * slideSpeed, 0f, 0f);
    }

    void StopSlide()
    {
        isSliding = false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}