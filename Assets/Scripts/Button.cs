using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void LoadScene(string Quack)
    {
        SceneManager.LoadScene(Quack);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
