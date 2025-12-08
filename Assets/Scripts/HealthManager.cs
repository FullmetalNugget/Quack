using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public static class GameStats {
    public static ushort kills = 0;
    public static ushort deaths = 0;
    public static ushort points = 0;
}

public class HealthManager : MonoBehaviour
{
    public byte maxHealth = 100;
    private byte maxDamage = 155;
    private byte currentHealth;

    [Header("SFX")]
    public AudioClip hitClip;
    public AudioClip dieClip;

    public TMP_Text healthText;
    public TMP_Text killsText;
    public TMP_Text deathsText;
    public TMP_Text pointsText;

    public ushort curPoints = 0;

    private playSFX sfx;

    private void Start()
    {
        currentHealth = maxHealth;

        UpdateText(healthText, currentHealth);
        UpdateText(pointsText, GameStats.points);
        UpdateText(killsText, GameStats.kills);
        UpdateDeathText();
    }

    private void Awake()
    {
        sfx = GetComponent<playSFX>();
    }

    public void TakeDamage(byte damage)
    {
        byte oldHealth = currentHealth;
        currentHealth -= damage;

        sfx?.Play(hitClip, 1, 1, 1);

        if (currentHealth == 0)
        {
            Die();
            return;
        }

        if (currentHealth > oldHealth || damage == maxDamage)
        {
            Die();
            return;
        }

        UpdateText(healthText, currentHealth);
    }

    private void Die()
    {
        GameStats.deaths++;
        UpdateDeathText();
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        sfx?.Play(dieClip, 1, 1, 1);
        curPoints = 0;
        yield return new WaitForSeconds(dieClip.length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // --- UI Updates ---

    public void UpdateText(TMP_Text text, ushort value)
    {
        text.text = value.ToString();
    }

    private void UpdateDeathText()
    {
        deathsText.text = GameStats.deaths + " X";
    }

    // --- Stats ---

    public void getKill()
    {
        GameStats.kills++;
        UpdateText(killsText, GameStats.kills);
    }

    public void AddPoints(ushort amount)
    {
        curPoints += amount;
        UpdateText(pointsText, (ushort)(curPoints+GameStats.points));
    }
}

