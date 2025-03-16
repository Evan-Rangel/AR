using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Vector3 playerPosition;

    public void Start()
    {
        playerPosition = transform.position;
    }
    public void Restart()
    {
        transform.position = playerPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
