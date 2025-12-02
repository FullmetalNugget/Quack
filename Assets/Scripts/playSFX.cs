using UnityEngine;

public class playSFX : MonoBehaviour
{
    public AudioSource audioSource;

    // Optional: with volume + random pitch
    public void Play(AudioClip clip, float volume, float pitchMin, float pitchMax)
    {
        if (audioSource == null || clip == null) return;

        float originalPitch = audioSource.pitch;
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(clip, volume);
        audioSource.pitch = originalPitch;
    }
}

