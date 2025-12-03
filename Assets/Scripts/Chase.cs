using UnityEngine;

public class Chase : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float verticalAmplitude = 1f;
    public float verticalFrequency = 2f;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        float yOffset = Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        transform.position = new Vector3(transform.position.x, startY + yOffset, transform.position.z);
    }
}