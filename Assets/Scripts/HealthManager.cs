using UnityEngine;
using TMPro; // Use TMPro if using TextMeshPro
using UnityEngine.SceneManagement;
using System.Collections;

public static class GameStats {
    public static ushort kills = 0;
    public static ushort deaths = 0;
}


public class HealthManager : MonoBehaviour
{
    public byte maxHealth = 100;
    private byte maxDamage = 155;
    private byte currentHealth;

    [Header("SFX")]
    public AudioClip hitClip;
    public AudioClip dieClip;

    public TMP_Text healthText; // Assign your UI Text here
    public TMP_Text killsText;
    public TMP_Text deathsText;

    private playSFX sfx;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }
    private void Awake() {
      sfx = GetComponent<playSFX>();
      killsText.text = GameStats.kills.ToString();
      updateDeathText(GameStats.deaths);
    }

    public void TakeDamage(byte damage)
    {
      byte oldHealth = currentHealth;
      currentHealth -= damage;
      sfx?.Play(hitClip, 1, 1, 1);

      // 1. Exact kill
      if (currentHealth == 0)
      {
          Die();
          return;
      }

      // 2. Overflow kill (damage exceeded health OR damage of 255 caused wrap)
      if (currentHealth > oldHealth || damage == maxDamage)
      {
          Die();
          return;
      }

      UpdateHealthText();
    }

    public void updateDeathText(ushort deaths) {
      deathsText.text = $"{GameStats.deaths.ToString()} X";
    }
    private IEnumerator DieRoutine()
    {
        sfx?.Play(dieClip, 1, 1, 1);

        // wait until the clip finishes
        yield return new WaitForSeconds(dieClip.length);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Die()
    {
      GameStats.deaths++;
      updateDeathText(GameStats.deaths);
      StartCoroutine(DieRoutine());
    }
    // Updates the UI text
    private void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString();
    }

    public void getKill() {
      GameStats.kills++;
      killsText.text = GameStats.kills.ToString();
    }

}

