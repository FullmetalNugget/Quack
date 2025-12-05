using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Credits : MonoBehaviour
{
    public float speed = 50f;
    public float returnDelay = 61f;
    public string sceneName = "UI";

    void Start()
    {
        StartCoroutine(ReturnAfterDelay());
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);
        SceneManager.LoadScene(sceneName);
    }
}