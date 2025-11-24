using UnityEngine;

public class Platfroms : MonoBehaviour
{
    public float amplitude = 2f;
    public float speed = 2f;

    private float startY;
    private float offset;

    void Start()
    {
        startY = transform.position.y;

        offset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * speed + offset) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}