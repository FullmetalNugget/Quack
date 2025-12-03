using UnityEngine;

public class SpiderBoss : MonoBehaviour
{
    [Header("Boss Settings")]
    public int maxHP = 100;
    private int currentHP;

    [Header("Spawning Settings")]
    public GameObject mobPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;

    private float spawnTimer;

    public GameObject bread;

    void Start()
    {
        currentHP = maxHP;
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnMob();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnMob()
    {
        if (mobPrefab != null && spawnPoints.Length > 0)
        {
            Transform chosenPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(mobPrefab, chosenPoint.position, chosenPoint.rotation);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        bread.SetActive(true);
    }
}
