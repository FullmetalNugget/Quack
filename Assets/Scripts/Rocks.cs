using UnityEngine;
using System.Collections;

public class Rocks : MonoBehaviour
{
    public GameObject rockPrefab;
    public Transform spawnCenter;
    public float range = 5f;
    public int numberOfRocks = 5;
    public float spawnInterval = 1f;
    public float rockLifetime = 4f;

    void Start()
    {
        StartCoroutine(SpawnRocks());
    }

    IEnumerator SpawnRocks()
    {
        while (true)
        {
            for (int i = 0; i < numberOfRocks; i++)
            {
                Vector3 spawnPos = rockPrefab.transform.position;
                spawnPos.x = Random.Range(spawnCenter.position.x - range, spawnCenter.position.x + range);

                GameObject rock = Instantiate(rockPrefab, spawnPos, rockPrefab.transform.rotation);
                Destroy(rock, rockLifetime);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}