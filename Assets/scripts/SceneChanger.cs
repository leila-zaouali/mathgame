using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public Animator doorAnimator;
    public AudioSource doorAudioSource;
    public AudioClip doorOpenSound;

    [Tooltip("Nom exact de l'etat d'animation declenche par le trigger 'Open' (visible dans l'Animator Controller)")]
    public string doorOpenStateName = "Open";

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
        float waitTime = 1f; // valeur de secours si jamais l'animator est absent

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");

            // On attend une frame pour laisser l'Animator passer dans le nouvel etat
            yield return null;

            // Recupere la duree reelle du clip d'animation en cours
            AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
            waitTime = stateInfo.length;
        }

        // Joue le son d'ouverture
        if (doorAudioSource != null && doorOpenSound != null)
            doorAudioSource.PlayOneShot(doorOpenSound);

        // Attend la fin reelle de l'animation avant de charger la scene
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(sceneToLoad);
    }
}