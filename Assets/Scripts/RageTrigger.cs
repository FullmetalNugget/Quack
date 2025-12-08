using UnityEngine;

public class RageTrigger : MonoBehaviour
{
    public Transform player;
    public float checkInterval = 2f;
    public float triggerRange = 10f;
    public float moveChance = 0.4f;
    public float moveSpeed = 7f;

    private float nextCheckTime = 0f;
    private bool isMoving = false;

    private Vector3 targetPos;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        FacePlayer();

        if (!isMoving)
        {
            TryStartMovement();
        }
        else
        {
            MoveTowardStoredPosition();
        }
    }

    void TryStartMovement()
    {
        if (Time.time < nextCheckTime)
            return;

        nextCheckTime = Time.time + checkInterval;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > triggerRange) return;

        if (Random.value < moveChance)
        {
            targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
            isMoving = true;
        }
    }

    void MoveTowardStoredPosition()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            isMoving = false;
        }
    }

    void FacePlayer()
    {
        Vector3 lookPos = new Vector3(player.position.x, player.position.y, transform.position.z);

        Vector3 direction = lookPos - transform.position;
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
