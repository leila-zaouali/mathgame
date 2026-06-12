using System.Collections;
using UnityEngine;

public class SpawnManager1 : MonoBehaviour
{
    public Transform defaultSpawn;

    IEnumerator Start()
    {
        GameObject player = null;

        // 🔥 attendre que le player existe
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            yield return null;
        }

        yield return null; // sécurité 1 frame

        Vector3 pos = defaultSpawn.position;

        if (ProgressManager.instance != null &&
            ProgressManager.instance.hasCheckpoint)
        {
            pos = ProgressManager.instance.checkpointPosition;
            Debug.Log("🎯 SPAWN CHECKPOINT APPLIED");
        }
        else
        {
            Debug.Log("🎯 SPAWN DEFAULT");
        }

        player.transform.position = pos;
    }
}