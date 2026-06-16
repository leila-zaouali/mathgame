using System.Collections;
using UnityEngine;

public class SpawnManager1 : MonoBehaviour
{
    public Transform defaultSpawn;
    private static bool spawned = false;

    void Awake()
    {
        Debug.Log("SpawnManager INSTANCE = " + gameObject.GetInstanceID());
        spawned = false;
    }

    IEnumerator Start()
    {
        if (spawned) yield break;
        spawned = true;

        // ✅ Attendre que GetProgress soit terminé
        while (!APIManager.progressLoaded) yield return null;

        GameObject player = null;
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        Vector3 pos = defaultSpawn.position;
        if (ProgressManager.instance != null && ProgressManager.instance.hasCheckpoint)
        {
            pos = ProgressManager.instance.checkpointPosition;
        }

        player.transform.position = pos;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("FINAL SPAWN APPLIED");
    }
}