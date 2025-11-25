using UnityEngine;
using TMPro; // Use TMPro if using TextMeshPro

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
        currentHealth -= damage;

        if ((currentHealth <= 0 || currentHealth > maxHealth) && damage !> maxDamage)
            currentHealth = 0;
            gameObject.SetActive(false);

        UpdateHealthText();
    }

    // Updates the UI text
    private void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString();
    }
}

