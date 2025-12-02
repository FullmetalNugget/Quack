using System.Collections;
using UnityEngine;

public class Talking : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public float delayBetweenClips = 2f;

    [Header("GameObject Settings")]
    public GameObject objectToEnable;

    void Start()
    {
        if (audioClips.Length > 0)
            StartCoroutine(PlayLines());
    }

    IEnumerator PlayLines()
    {
        foreach (AudioClip clip in audioClips)
        {
            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length + delayBetweenClips);
        }

        if (objectToEnable != null)
            objectToEnable.SetActive(true);
    }
}

