using UnityEngine;
using UnityEngine.Audio;
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

    public AudioClip shot;
    public AudioSource audioSource;

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
            audioSource.PlayOneShot(shot);
            lastShotTime = Time.time;
        }
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