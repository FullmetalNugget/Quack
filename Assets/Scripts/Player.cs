using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 10f;
    public float runSpeed = 15f;
    public float jumpForce = 7f;
    public float slideSpeed = 14f;
    public float slideDuration = 0.5f;

    [Header("QUake movement params")]
    public float accelSpeed = 4.0f;
    public float maxSpeed = 10.0f;
    public float maxAirSpeed = 5.0f;

    public float friction = 2.0f;
    public float airControl = 0.2f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isSliding;
    private float slideTimer;
    private float accel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezePositionZ;
    }

    void sv_accelerate(Rigidbody rb, Vector3 wishDir, float wishSpeed, float accelerate) {
      Vector3 velocity = rb.linearVelocity;  // get current velocity

      // Project current velocity onto desired direction
      float currentSpeed = Vector3.Dot(velocity, wishDir);

      // How much speed we need to add
      float addSpeed = wishSpeed - currentSpeed;
      if (addSpeed <= 0)
          return;

      // Determine acceleration this frame
      float accelSpeed = accelerate * Time.deltaTime * wishSpeed;
      if (accelSpeed > addSpeed)
          accelSpeed = addSpeed;

      // Apply acceleration in the desired direction
      velocity += accelSpeed * wishDir;

      rb.linearVelocity = velocity; // write it back to the Rigidbody
    }
    void ApplyFriction(Rigidbody rb, float friction)
    {
      Vector3 vel = rb.linearVelocity;
      float speed = vel.magnitude;

      if (speed < 0.001f) return; // already almost stopped

      float drop = speed * friction * Time.fixedDeltaTime;
      float newSpeed = Mathf.Max(speed - drop, 0);

      rb.linearVelocity = vel * (newSpeed / speed);
    }

    void Update()
    {
        CheckGrounded();

        Vector3 wishDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isGrounded = CheckGrounded();
       
        if (!isSliding)
        {


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

        if (isGrounded) {
          if (!Input.GetKeyDown(KeyCode.LeftControl)) {ApplyFriction(rb, friction);}
          sv_accelerate(rb, wishDir, maxSpeed, accelSpeed);
        }
        else {
           
          sv_accelerate(rb, wishDir, maxAirSpeed, accelSpeed * airControl);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            //StartSlide(moveInput);
        }

    }

    bool CheckGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
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
