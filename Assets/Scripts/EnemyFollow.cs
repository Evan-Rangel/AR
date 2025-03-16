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

    void Start()
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

        //Shoot Bullet
        //ShootAtPlayer();
    }

    /*
    void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;

        if (bulletTime > 0)
        {
            return;
        }

        bulletTime = timer;

        GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.position, spawnPoint.rotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();

        if (bulletRig != null)
        {
            bulletRig.AddForce(spawnPoint.forward * bulletspeed, ForceMode.Impulse);
        }

        Destroy(bulletObj, 5f);
    }
    */

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Jugador alcanzado!");
            other.gameObject.SetActive(false); 
        }
    }
}
