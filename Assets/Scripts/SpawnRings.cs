using System.Collections;
using UnityEngine;

public class SpawnRings : MonoBehaviour
{
    public GameObject ringPrefab;  
    public Transform spawnPoint;   
    public Vector3 spawnRange = new Vector3(0.04f, 0f, 0.04f); 

    private GameObject currentRing;
    [SerializeField] float enemyRespawnTime;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform target;
    
    GameObject currentEnemy;
    void Start()
    {
        SpawnObject();
        StartCoroutine(SpawnEnemy());
    }
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if (currentEnemy != null)
                Destroy(currentEnemy);
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRange.x, spawnRange.x),
                0.015f,
                Random.Range(-spawnRange.z, spawnRange.z)
            );

            currentEnemy= Instantiate(enemyPrefab, spawnPoint.position + randomOffset, Quaternion.identity, target);
            yield return new WaitForSeconds(Random.Range(5, enemyRespawnTime));
        }
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
