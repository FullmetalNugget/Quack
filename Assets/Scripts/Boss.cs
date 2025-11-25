using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum BossStage { Stage1, Stage2, Stage3 }
    public BossStage currentStage = BossStage.Stage1;

    [Header("Boss Settings")]
    public float maxHealth = 100f;
    public float health;

    [Header("Jump Attack")]
    public float jumpSpeedStage2 = 7f;
    public float jumpSpeedStage3 = 10f;

    [Header("Hands")]
    public Transform[] hands;
    public float handSpeedStage1 = 5f;
    public float handSpeedStage2 = 7f;
    public float handSpeedStage3 = 9f;
    public float handCheckInterval = 1.5f;
    public float handMoveChance = 0.5f;

    [Header("Player")]
    public Transform player;

    private Vector3[] originalHandPositions;
    private Vector3[] handTargetPositions;
    private bool[] isHandMoving;
    private float nextHandCheckTime;

    void Start()
    {
        health = maxHealth;

        originalHandPositions = new Vector3[hands.Length];
        handTargetPositions = new Vector3[hands.Length];
        isHandMoving = new bool[hands.Length];

        for (int i = 0; i < hands.Length; i++)
        {
            originalHandPositions[i] = hands[i].localPosition;
            handTargetPositions[i] = hands[i].position;
            isHandMoving[i] = false;
        }

        nextHandCheckTime = Time.time + handCheckInterval;
    }

    void Update()
    {
        if (player == null) return;

        HandleHands();

        StageBehavior();
    }

    void StageBehavior()
    {
        switch (currentStage)
        {
            case BossStage.Stage1:
                break;

            case BossStage.Stage2:
                JumpTowardsPlayer(jumpSpeedStage2);
                break;

            case BossStage.Stage3:
                JumpTowardsPlayer(jumpSpeedStage3);
                break;
        }
    }

    void JumpTowardsPlayer(float speed)
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    void HandleHands()
    {
        float currentHandSpeed = handSpeedStage1;
        if (currentStage == BossStage.Stage2) currentHandSpeed = handSpeedStage2;
        if (currentStage == BossStage.Stage3) currentHandSpeed = handSpeedStage3;

        if (Time.time >= nextHandCheckTime)
        {
            nextHandCheckTime = Time.time + handCheckInterval;

            for (int i = 0; i < hands.Length; i++)
            {
                if (!isHandMoving[i] && Random.value < handMoveChance)
                {
                    handTargetPositions[i] = new Vector3(player.position.x, player.position.y, hands[i].position.z);
                    isHandMoving[i] = true;
                }
            }
        }

        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].position = Vector3.MoveTowards(hands[i].position, handTargetPositions[i], currentHandSpeed * Time.deltaTime);

            if (isHandMoving[i] && Vector3.Distance(hands[i].position, handTargetPositions[i]) < 0.05f)
            {
                handTargetPositions[i] = transform.position + originalHandPositions[i];
                isHandMoving[i] = false;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
            return;
        }

        if (health <= maxHealth - 30f && currentStage == BossStage.Stage1)
        {
            currentStage = BossStage.Stage2;
        }
        else if (health <= maxHealth - 60f && currentStage == BossStage.Stage2)
        {
            currentStage = BossStage.Stage3;
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            TakeDamage(10f);
        }
    }
}