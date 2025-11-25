using UnityEngine;
using UnityEngine.UI; // UI Button

public class KYSTest : MonoBehaviour
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
        playerHealth.TakeDamage(damage);
    }
}

