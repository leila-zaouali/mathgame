using UnityEngine;

public class PlayerSpawnA : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject spawn = GameObject.Find("spawn");

        if (player != null && spawn != null)
        {
            player.transform.position = spawn.transform.position;
        }
    }
}
