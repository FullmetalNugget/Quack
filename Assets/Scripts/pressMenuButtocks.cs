using UnityEngine;
using UnityEngine.UI; // UI Button
using UnityEngine.SceneManagement;

public class pressMenu : MonoBehaviour
{
    private UnityEngine.UI.Button btn; // fully qualified
    public HealthManager playerHealth; // assign your player object
    public byte damage;

    private void Awake()
    {
        btn = GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(OnClick);

    }

    private void OnClick()
    {
      SceneManager.LoadScene("UI");
    }
}

