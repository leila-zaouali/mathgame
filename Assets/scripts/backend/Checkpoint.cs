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

        string scene = SceneManager.GetActiveScene().name;
        Vector3 pos = transform.position;

        // ✅ Sauvegarder sur Firebase uniquement
        if (APIManager.instance != null)
            APIManager.instance.SaveCheckpoint(scene, pos);

        // ✅ Mettre à jour ProgressManager directement
        if (ProgressManager.instance != null)
            ProgressManager.instance.SetCheckpoint(scene, pos.x, pos.y, pos.z);

        Debug.Log("📍 CHECKPOINT SAUVEGARDÉ: " + scene);
    }
}