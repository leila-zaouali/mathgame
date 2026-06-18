using UnityEngine;

public class IntroAudioDelay : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            // Lance le son automatiquement après 2 secondes
            audioSource.PlayDelayed(1.0f);
        }
    }
}
