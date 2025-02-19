using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public UnityEvent playerMove;
    private Rigidbody2D playerBody;
    public TextMeshProUGUI scoreText;
    private Vector3 startingPosition;
    public GameObject normalCanvas;
    public GameObject gameOverCanvas;
    public TextMeshProUGUI gameOverScoreText;
    public LogicScript logicScript;

    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 60 FPS
        playerBody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

    }



    // FixedUpdate is called 50 times a second
    void  FixedUpdate()
    {

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
        // foreach (Transform eachChild in obstacles.transform)
        // {
        //     eachChild.transform.localPosition = eachChild.GetComponent<ObstacleMovement>().startPosition;
        // }
        // reset score
        //JumpOverObstacle.score = 0;
        logicScript.score = 0;

    }

    public void MoveCheck(int value)
    {
        Debug.Log("Move check called with value: " + value);
        if (value == 0)
        {
            moving = false;
        }
        else if (value == -1)
        {
            moving = true;
            MoveLeft();
        }
        else if (value == 1)
        {
            moving = true;
            MoveRight();
        }
    }
    public void MoveLeft()
    {
        // TODO: Connect to some level/tile manager
        Debug.Log("Moving player left");
    }

    public void MoveRight()
    {
        // TODO: Connect to some level/tile manager
        Debug.Log("Moving player right"); 
    }
}
