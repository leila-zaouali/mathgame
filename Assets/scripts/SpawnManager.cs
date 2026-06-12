using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawn;
    public Transform spawnPoint;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Transform chosen = spawnPoint; // default = début niveau

        // 🔥 si joueur revient après progression
        if (ProgressManager.instance != null &&
            ProgressManager.instance.game1Completed)
        {
            chosen = spawn; // retour / checkpoint
        }
        //if (GameState.returnFromGame1)
        //{
        //    chosen = spawn;
        //    GameState.returnFromGame1 = false; // reset
        //}

        player.transform.position = chosen.position;
        player.transform.rotation = chosen.rotation;

        Debug.Log("🎯 Spawn utilisé: " + chosen.name);
    }
}