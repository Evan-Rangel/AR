using UnityEngine;

public class BulletScript : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5f); // La bala se destruye si no impacta
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¡Jugador alcanzado!");
            collision.gameObject.SetActive(false); 
            Destroy(gameObject); 
        }
    }
}
