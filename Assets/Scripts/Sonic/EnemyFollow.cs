using System.Collections;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotationSpeed = 2f;
    public float stopDistance = 0.5f;

    private Transform player;

    [SerializeField] private float timer = 5;
    private float bulletTime;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float bulletspeed;

    void Awake()
    {

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("Player encontrado");
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con el tag 'Player'");
        }
        
    }
    private void Start()
    {
        StartCoroutine(Shoot());
        StartCoroutine(Kill()); 
    }
    void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude > stopDistance)
        {
            
            Vector3 move = direction.normalized * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World); 

            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

    }
    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(timer);

            GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.position, this.transform.rotation);
            Rigidbody bulletRb = bulletObj.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.AddForce(bulletObj.transform.forward * bulletspeed, ForceMode.Impulse);

            }
        }
    }
    IEnumerator Kill()
    {
        yield return new WaitForSeconds(Random.Range(5,10));
        Destroy(gameObject);
        
    }
 /*   void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false); 
        }
    }*/
}
