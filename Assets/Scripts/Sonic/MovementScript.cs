using System.Transactions;
using UnityEngine;
using UnityEngine.UI; 

[RequireComponent(typeof(Rigidbody))]
public class MovementScript : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 720f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public float fallMultiplier = 1.5f;

    private Rigidbody rb;
    private Vector3 movementInput;
    private bool isGrounded;
    private bool jumpRequested;

    public Joystick joy;
    public Vector3 playerPosition;

    public int score = 0;
    private int highScore;
    public TMPro.TextMeshProUGUI scoreText;
    public GameObject gameOver;
    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        playerPosition = transform.position;

        // Cargar el highscore guardado
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();
    }

    public void Restart()
    {
        
        if (score > highScore)
        {
            highScore = score;
        }

        
        score = 0;
        UpdateScoreUI();

        
        transform.position = playerPosition;
        gameObject.SetActive(true);
    }

    void Update()
    {
        // Movimiento con joystick
        float joyHorizontal = joy.Horizontal;
        float joyVertical = joy.Vertical;
        movementInput = new Vector3(joyHorizontal, 0f, joyVertical);

        // Chequeo de suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        Vector3 direction = movementInput.normalized;

        if (direction.magnitude > 0.1f)
        {
            Vector3 move = transform.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(move);

            // Rotar hacia donde se mueve
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime));
        }

        // Salto
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }

        // Caída más suave
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Puntaje: " + score);

        
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score + "\nHigh Score: " + highScore;
        }
    }

    public void OnJumpButtonPressed()
    {
        if (isGrounded)
        {
            jumpRequested = true;
        }
    }

    public void ActivateGravity()
    {
        rb.useGravity = true;
    }

    public void DesactivateGravity()
    {
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            gameOver.SetActive(true);
            Destroy(other.gameObject);
            gameObject.SetActive(false);
        }
        if (other.CompareTag("Enemy"))
        {
            gameOver.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
