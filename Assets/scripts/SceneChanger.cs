using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public Animator doorAnimator;
    public AudioSource doorAudioSource; // ✅ AJOUTE
    public AudioClip doorOpenSound;     // ✅ AJOUTE

    private bool isChanging = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isChanging)
        {
            isChanging = true;
            StartCoroutine(OpenDoorAndLoad());
        }
    }

    IEnumerator OpenDoorAndLoad()
    {
        if (doorAnimator != null)
            doorAnimator.SetTrigger("Open");

        // ✅ Jouer le son
        if (doorAudioSource != null && doorOpenSound != null)
            doorAudioSource.PlayOneShot(doorOpenSound);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneToLoad);
    }
}