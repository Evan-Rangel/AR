using UnityEngine;

public class RingScript : MonoBehaviour
{
    public float rotationSpeed = 50f; 

    void Update()
    {
        // Rotar
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Moneda recogida!");

            // Aumentar el puntaje 
            MovementScript player = other.GetComponent<MovementScript>();
            if (player != null)
            {
                player.AddScore(1);
            }

            // Buscar el SpawnRings y generar otra moneda
            SpawnRings spawner = FindObjectOfType<SpawnRings>();
            if (spawner != null)
            {
                spawner.CollectRing(gameObject);
            }
        }
    }
}
