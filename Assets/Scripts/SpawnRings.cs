using UnityEngine;

public class SpawnRings : MonoBehaviour
{
    public GameObject ringPrefab;  
    public Transform spawnPoint;   
    public Vector3 spawnRange = new Vector3(0.04f, 0f, 0.04f); 

    private GameObject currentRing; 

    void Start()
    {
        SpawnObject();
    }

    void SpawnObject()
    {
        if (ringPrefab != null && spawnPoint != null)
        {
            
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRange.x, spawnRange.x),
                spawnRange.y,
                Random.Range(-spawnRange.z, spawnRange.z)
            );

            
            currentRing = Instantiate(ringPrefab, spawnPoint.position + randomOffset, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Falta asignar el prefab de la moneda o el punto de spawn.");
        }
    }

    public void CollectRing(GameObject ring)
    {
        Destroy(ring); 
        SpawnObject(); 
    }
}
