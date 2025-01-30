using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float maxSpeed = 20;
    private Rigidbody2D playerBody;
    public float upSpeed = 10;
    private bool onGroundState = true;
    public TextMeshProUGUI scoreText;
    public GameObject obstacles;
    private Vector3 startingPosition;
    public GameObject normalCanvas;
    public GameObject gameOverCanvas;
    public TextMeshProUGUI gameOverScoreText;
    public JumpOverObstacle JumpOverObstacle;
    private bool jumping = false;
    private bool canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        playerBody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called when the Collider component of the GameObject containing this script hits something
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) onGroundState = true;
    }

    // Called when the RigidBody2D component of the GameObject containing this script hits a Collier2D trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collision with obstacle detected!");
            Time.timeScale = 0.0f;
            GameOver();
        }
    }

    // FixedUpdate is called 50 times a second
    void  FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (playerBody.linearVelocity.magnitude < maxSpeed)
                    playerBody.AddForce(movement * speed);
        }

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // stop
            playerBody.linearVelocity = Vector2.zero;
        }

        // jump
        if (Input.GetKeyDown("space") && onGroundState)
        {
            playerBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
        }
    }

    public void GameOver()
    {
        normalCanvas.SetActive(false);
        gameOverScoreText.text = scoreText.text;
        gameOverCanvas.SetActive(true);
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame()
    {
        // reset position
        //playerBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        playerBody.transform.position = startingPosition;
        // reset sprite direction
        //faceRightState = true;
        //marioSprite.flipX = false;
        // reset canvas
        normalCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        // reset score
        scoreText.text = "Score: 0";
        // reset obstacles
        foreach (Transform eachChild in obstacles.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<ObstacleMovement>().startPosition;
        }
        // reset score
        JumpOverObstacle.score = 0;

    }
}
