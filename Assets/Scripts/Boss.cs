using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform[] spikes;
    public float checkInterval = 5f;
    public float popHeight = 2f;
    public float popSpeed = 5f;
    public float chance = 0.4f;

    private float timer;
    private Vector3[] originalPos;

    void Start()
    {
        originalPos = new Vector3[spikes.Length];
        for (int i = 0; i < spikes.Length; i++)
            originalPos[i] = spikes[i].position;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            TryPopSpikes();
            timer = checkInterval;
        }
    }

    void TryPopSpikes()
    {
        foreach (Transform spike in spikes)
        {
            if (Random.value <= chance)
                StartCoroutine(PopSpike(spike));
        }
    }

    System.Collections.IEnumerator PopSpike(Transform spike)
    {
        Vector3 upPos = spike.position + Vector3.up * popHeight;

        while (Vector3.Distance(spike.position, upPos) > 0.01f)
        {
            spike.position = Vector3.MoveTowards(
                spike.position,
                upPos,
                popSpeed * Time.deltaTime
            );
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        int index = System.Array.IndexOf(spikes, spike);
        Vector3 downPos = originalPos[index];

        while (Vector3.Distance(spike.position, downPos) > 0.01f)
        {
            spike.position = Vector3.MoveTowards(
                spike.position,
                downPos,
                popSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
}