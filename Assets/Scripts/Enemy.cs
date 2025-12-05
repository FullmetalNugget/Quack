using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public int maxHealth = 30;

    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public byte damage = 10;

    private float lastAttackTime;
    private Animator animator;

    private int currentHealth;
    private Transform player;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            if (animator != null)
                animator.SetTrigger("Attack");

            player.GetComponent<HealthManager>()?.TakeDamage(damage);

            lastAttackTime = Time.time;
        }
    }

    public void enemyTakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        player.GetComponent<HealthManager>()?.getKill();
    }
}
