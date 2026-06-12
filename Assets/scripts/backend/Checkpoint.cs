using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (used) return;

        used = true;

        ProgressManager.instance.checkpointPosition = transform.position;
        ProgressManager.instance.checkpointScene = SceneManager.GetActiveScene().name;
        ProgressManager.instance.hasCheckpoint = true;

        PlayerPrefs.SetString("cp_scene", ProgressManager.instance.checkpointScene);
        PlayerPrefs.SetFloat("cp_x", transform.position.x);
        PlayerPrefs.SetFloat("cp_y", transform.position.y);
        PlayerPrefs.SetFloat("cp_z", transform.position.z);

        PlayerPrefs.Save();

        Debug.Log("📍 CHECKPOINT SAUVEGARDÉ AVEC SCÈNE");
    }
}