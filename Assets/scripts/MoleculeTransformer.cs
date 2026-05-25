//using UnityEngine;

//public class MoleculeTransformer : MonoBehaviour
//{
//    public GameObject waterDropPrefab;
//    public Transform handPoint;

//    private GameObject currentDrop;
//    private bool isInHand = false;

//    void Update()
//    {
//        if (GameInput.instance.UsePressed())
//        {
//            if (Inventory.instance == null) return;

//            if (!isInHand)
//            {
//                if (Inventory.instance.waterDropCount <= 0) return;

//                Inventory.instance.waterDropCount--;

//                currentDrop = Instantiate(waterDropPrefab, handPoint.position, handPoint.rotation);
//                currentDrop.transform.SetParent(handPoint);

//                isInHand = true;
//            }
//            else
//            {
//                Inventory.instance.waterDropCount++;
//                Destroy(currentDrop);
//                currentDrop = null;
//                isInHand = false;
//            }
//        }
//    }
//}