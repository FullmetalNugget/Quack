using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string playerTag = "Player";
    public HealthManager hm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            LoadNewScene();
        }
    }

    public void LoadNewScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        GameStats.points+=hm.curPoints;
        SceneManager.LoadSceneAsync(nextSceneIndex);
    }
}
