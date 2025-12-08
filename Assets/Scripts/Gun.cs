using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.2f;
    public float range = 50f;
    public int damage = 10;

    private float lastShotTime;
    private Vector3 lastAimDirection = Vector3.right;

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
        if (attackAction.WasPressedThisFrame() && Time.time - lastShotTime >= fireRate)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPos;

        if (!TryGetPointerWorldPosition(out worldPos))
            return Vector3.zero;

        return worldPos;
    }

    bool TryGetPointerWorldPosition(out Vector3 worldPos)
    {
        Vector2 pointerScreenPos;

        if (Mouse.current != null)
        {
            pointerScreenPos = Mouse.current.position.ReadValue();
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            pointerScreenPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else
        {
            worldPos = Vector3.zero;
            return false;
        }

        Ray ray = Camera.main.ScreenPointToRay(pointerScreenPos);

        Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);

        if (xyPlane.Raycast(ray, out float distance))
        {
            worldPos = ray.GetPoint(distance);
            worldPos.z = -6.1f;
            return true;
        }

        worldPos = Vector3.zero;
        return false;
    }

    void Shoot()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 direction;

        if (mousePos != Vector3.zero)
        {
            direction = (mousePos - transform.position).normalized;
            lastAimDirection = direction;
        }
        else
        {
            direction = lastAimDirection;
        }

        if (direction == Vector3.zero)
            return;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                var enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.enemyTakeDamage(damage);
                    return;
                }

                var boss = hit.collider.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.TakeDamage(damage);
                    return;
                }
            }
        }

        Debug.DrawRay(transform.position, direction * range, Color.red, 0.2f);
    }
}
