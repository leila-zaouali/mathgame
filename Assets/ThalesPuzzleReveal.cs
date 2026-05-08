using UnityEngine;

public class ThalesPuzzleReveal : MonoBehaviour
{
    public Transform pointD;          // point que le joueur déplace
    public Transform correctD;        // position correcte (invisible)
    public GameObject hiddenKey;      // clé cachée
    public GameObject cubeToLift;     // cube au sol

    public float tolerance = 0.3f;

    private bool solved = false;

    void Update()
    {
        if (solved) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    CheckSolution();
                }
            }
        }
    }

    void CheckSolution()
    {
        float dist = Vector3.Distance(pointD.position, correctD.position);

        if (dist <= tolerance)
        {
            solved = true;

            Debug.Log("✔ Puzzle résolu !");

            // 🔥 faire monter le cube
            cubeToLift.transform.position += Vector3.up * 1.5f;

            // 🔑 afficher la clé
            hiddenKey.SetActive(true);
        }
        else
        {
            Debug.Log("❌ Mauvaise position de D");
        }
    }
}