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

        string id = PlayerSession.UserId;

        PlayerPrefs.SetString(id + "_cp_scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat(id + "_cp_x", transform.position.x);
        PlayerPrefs.SetFloat(id + "_cp_y", transform.position.y);
        PlayerPrefs.SetFloat(id + "_cp_z", transform.position.z);
        PlayerPrefs.SetInt(id + "_cp_active", 1);

        PlayerPrefs.Save();

        Debug.Log("📍 CHECKPOINT SAUVEGARDÉ POUR " + id);
    }
}