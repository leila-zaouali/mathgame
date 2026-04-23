using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToScene1 : MonoBehaviour
{
    public int requiredH2O = 3;
    public string scene1Name = "Scene1";

    private Inventory inventory;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            inventory = player.GetComponent<Inventory>();
        }
    }

    void Update()
    {
        if (inventory != null)
        {
            if (inventory.h2oCount >= requiredH2O)
            {
                SceneManager.LoadScene(scene1Name);
            }
        }
    }
}
