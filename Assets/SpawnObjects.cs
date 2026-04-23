using UnityEngine;

public class SpawnInAir3D : MonoBehaviour
{
    public GameObject objectToSpawn;

    public int count = 30;

    public Vector3 areaSize = new Vector3(10f, 5f, 10f);

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-areaSize.x, areaSize.x),
                Random.Range(-areaSize.y, areaSize.y),
                Random.Range(-areaSize.z, areaSize.z)
            );

            Vector3 spawnPos = transform.position + randomOffset;

            GameObject obj = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);

            // ✅ garder exactement le même nom (sans _i ni Clone)
            obj.name = objectToSpawn.name;
        }
    }
}