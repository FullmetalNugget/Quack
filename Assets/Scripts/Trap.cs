using UnityEngine;
using UnityEngine.SceneManagement;

public class Trap : MonoBehaviour


{

    public byte damage = 50;
    public float attackCooldown = 2.0f;
    private float lastAttackTime;

    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      player = GameObject.FindWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          TryAttack();

        }
    }
    void TryAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            player.GetComponent<HealthManager>()?.TakeDamage(damage);

            lastAttackTime = Time.time;
        }
    }
}
