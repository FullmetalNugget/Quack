using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("General Settings")]
    public int maxHP = 1000;
    public int currentHP;
    public int bossStage = 1;

    [Header("Stage Settings")]
    public int[] stageThresholds;
    public float stageIntervalMultiplier = 0.8f;
    public float stageChanceIncrement = 0.1f;

    [Header("Spikes Attack")]
    public Transform[] spikePositions;
    public float spikePopHeight = 2f;
    public float spikeUpDuration = 1f;
    public float spikeDownDuration = 1f;
    public float spikeInterval = 5f;
    public float spikeChance = 0.3f;

    [Header("Hands Attack")]
    public Transform[] hands;
    public float handHoverHeight = 3f;
    public float handSpeed = 5f;
    public float handAttackChance = 0.5f;
    public float handAttackInterval = 5f;

    private float handTimer;
    private Vector3[] handStartPositions;
    private bool[] handIsAttacking;
    private Vector3[] handTargetPositions;

    [Header("Jump Attack")]
    public float jumpForce = 10f;
    public float jumpForwardForce = 5f;
    public float jumpCooldown = 6f;
    private float jumpTimer;
    private Rigidbody rb;

    private float spikeTimer;

    void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody>();

        handStartPositions = new Vector3[hands.Length];
        handIsAttacking = new bool[hands.Length];
        handTargetPositions = new Vector3[hands.Length];
        for (int i = 0; i < hands.Length; i++)
        {
            handStartPositions[i] = hands[i].position;
            handIsAttacking[i] = false;
        }
    }

    void Update()
    {
        HandleStage();
        HandleSpikes();
        HandleHands();
        HandleJump();
    }

    void HandleJump()
    {
        if (bossStage < 2) return;

        jumpTimer += Time.deltaTime;
        if (jumpTimer >= jumpCooldown)
        {
            jumpTimer = 0f;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 targetPos = player.transform.position;
                Vector3 direction = (targetPos - transform.position).normalized;

                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

                Vector3 force = new Vector3(direction.x * jumpForwardForce, jumpForce, direction.z * jumpForwardForce);
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }

    void HandleStage()
    {
        for (int i = stageThresholds.Length - 1; i >= 0; i--)
        {
            if (currentHP <= stageThresholds[i] && bossStage <= i)
            {
                bossStage = i + 2;
                Debug.Log("Boss advanced to stage: " + bossStage);
            }
        }
    }

    void HandleSpikes()
    {
        spikeTimer += Time.deltaTime;
        float currentInterval = spikeInterval * Mathf.Pow(stageIntervalMultiplier, bossStage - 1);

        if (spikeTimer >= currentInterval)
        {
            spikeTimer = 0f;
            foreach (Transform spike in spikePositions)
            {
                if (Random.value < spikeChance + (bossStage - 1) * stageChanceIncrement)
                {
                    StartCoroutine(PopSpike(spike));
                }
            }
        }
    }

    IEnumerator PopSpike(Transform spike)
    {
        Vector3 startPos = spike.position;
        Vector3 endPos = startPos + Vector3.up * spikePopHeight;

        float t = 0f;
        while (t < spikeUpDuration)
        {
            spike.position = Vector3.Lerp(startPos, endPos, t / spikeUpDuration);
            t += Time.deltaTime;
            yield return null;
        }
        spike.position = endPos;

        t = 0f;
        while (t < spikeDownDuration)
        {
            spike.position = Vector3.Lerp(endPos, startPos, t / spikeDownDuration);
            t += Time.deltaTime;
            yield return null;
        }
        spike.position = startPos;
    }

    void HandleHands()
    {
        handTimer += Time.deltaTime;
        if (handTimer >= handAttackInterval)
        {
            handTimer = 0f;

            for (int i = 0; i < hands.Length; i++)
            {
                if (!handIsAttacking[i] && Random.value < handAttackChance)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                    {
                        handTargetPositions[i] = player.transform.position;
                        handIsAttacking[i] = true;
                        StartCoroutine(MoveHand(hands[i], i));
                    }
                }
            }
        }

        for (int i = 0; i < hands.Length; i++)
        {
            if (!handIsAttacking[i])
            {
                hands[i].position = Vector3.Lerp(hands[i].position, handStartPositions[i], Time.deltaTime * 2f);
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) hands[i].LookAt(player.transform);
            }
        }
    }

    IEnumerator MoveHand(Transform hand, int index)
    {
        Vector3 target = handTargetPositions[index];
        Vector3 start = hand.position;

        float t = 0f;
        float duration = Vector3.Distance(start, target) / handSpeed;
        while (t < duration)
        {
            hand.position = Vector3.Lerp(start, target, t / duration);
            t += Time.deltaTime;
            hand.LookAt(target);
            yield return null;
        }
        hand.position = target;

        yield return new WaitForSeconds(0.2f);

        start = hand.position;
        target = handStartPositions[index];
        t = 0f;
        duration = Vector3.Distance(start, target) / handSpeed;
        while (t < duration)
        {
            hand.position = Vector3.Lerp(start, target, t / duration);
            t += Time.deltaTime;
            hand.LookAt(target);
            yield return null;
        }
        hand.position = target;
        handIsAttacking[index] = false;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
    }
}