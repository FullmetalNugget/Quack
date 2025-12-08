using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float range = 50f;
    public int damage = 10;
    [HideInInspector] public Vector3 direction;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(startPos, transform.position) >= range)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var e = other.GetComponent<Enemy>();
            if (e != null) e.enemyTakeDamage(damage);
        }

        if (other.CompareTag("Boss"))
        {
            var b = other.GetComponent<BossController>();
            if (b != null) b.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}