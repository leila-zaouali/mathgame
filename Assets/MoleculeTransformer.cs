using UnityEngine;

public class MoleculeTransformer : MonoBehaviour
{
    public GameObject waterDropPrefab;
    public Transform handPoint;

    private GameObject currentDrop;
    private bool isInHand = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Inventory.instance == null) return;

            // 🔴 SI PAS EN MAIN → METTRE EN MAIN
            if (!isInHand)
            {
                if (Inventory.instance.waterDropCount <= 0) return;

                Inventory.instance.waterDropCount--;

                currentDrop = Instantiate(
                    waterDropPrefab,
                    handPoint.position,
                    handPoint.rotation
                );

                currentDrop.transform.SetParent(handPoint);

                isInHand = true;

                Debug.Log("💧 Goutte en main");
            }
            // 🔴 SI DEJA EN MAIN → RETOUR INVENTAIRE
            else
            {
                Inventory.instance.waterDropCount++;

                Destroy(currentDrop);
                currentDrop = null;

                isInHand = false;

                Debug.Log("📦 Retour inventaire");
            }
        }
    }
}