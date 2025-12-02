using UnityEngine;
using TMPro; // Use TMPro if using TextMeshPro
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public byte maxHealth = 100;
    private byte maxDamage = 155;
    private byte currentHealth;

    public TMP_Text healthText; // Assign your UI Text here

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    // Call this function to take damage
   public void TakeDamage(byte damage)
  {
    if (damage >= maxDamage)
        return;

    int result = currentHealth - damage;
    currentHealth = (byte)Mathf.Max(result, 0);

    if (currentHealth == 0)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    UpdateHealthText();
  }
    // Updates the UI text
    private void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString();
    }
}

