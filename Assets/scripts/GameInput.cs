using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput instance;

    [Header("Touches")]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode useKey = KeyCode.P;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool InteractPressed()
    {
        return Input.GetKeyDown(interactKey);
    }

    public bool InventoryPressed()
    {
        return Input.GetKeyDown(inventoryKey);
    }

    public bool UsePressed()
    {
        return Input.GetKeyDown(useKey);
    }

    public bool ClickPressed()
    {
        return Input.GetMouseButtonDown(0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E PRESSED DETECTED");
        }
    }
}