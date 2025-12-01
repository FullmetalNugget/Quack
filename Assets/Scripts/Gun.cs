using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.2f;
    public float range = 50f;
    public int damage = 10;

    private float lastShotTime;

    private PlayerInput playerInput;
    private InputAction attackAction;
    private InputAction mousePositionAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        attackAction = playerInput.actions["Attack"];
        mousePositionAction = playerInput.actions["MousePosition"];
    }

    void Update()
    {
        RotateTowardsMouse();

        if (attackAction.WasPressedThisFrame() && Time.time - lastShotTime >= fireRate)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(-90f, 0f, angle);
    }

    Vector3 GetMouseWorldPosition()
    {
        if (Mouse.current == null)
            return Vector3.zero;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);

        if (xyPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPos = ray.GetPoint(distance);
            worldPos.z = -6.1f;
            return worldPos;
        }

        return Vector3.zero;
    }

    void Shoot()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 direction = (mousePos - transform.position).normalized;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, range))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        Debug.DrawRay(transform.position, direction * range, Color.red, 0.2f);
    }
}