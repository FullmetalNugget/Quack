using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    public float MoveSpeed = 10f;
    public float JumpForce = 5f;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
