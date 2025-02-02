using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Ally[] allies;
    public Boss boss;
    public Healer healer;
    public GameObject normalCanvas;
    public GameObject gameOverCanvas;
    public TextMeshProUGUI scoreText;

    [System.NonSerialized]
    public int score = 0; // we don't want this to show up in the inspector
    public TextMeshProUGUI gameOverScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.health <= 0)
        {
            Debug.Log("You won!");
            addScore(100);
            Time.timeScale = 0f;
            boss.health = boss.maxHealth; // temporary to stop infinite score adding
            GameWin();
        }
        else if (AllAlliesDead())
        {
            Debug.Log("Game Over!");
            Time.timeScale = 0f;
            GameOver();
        }
    }


    public void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score.ToString();
    }

    bool AllAlliesDead()
    {
        foreach (Ally ally in allies)
        {
            if (ally.health > 0) return false;
        }
        return true;
    }

    public void GameOver()
    {
        normalCanvas.SetActive(false);
        gameOverScoreText.text = scoreText.text;
        gameOverCanvas.SetActive(true);
    }

    public void GameWin()
    {
        normalCanvas.SetActive(false);
        gameOverScoreText.text = scoreText.text;
        gameOverCanvas.SetActive(true);
        // NOTE: This may be inefficient
        TextMeshProUGUI gameOverText = gameOverCanvas.transform.Find("GameOverText").GetComponent<TextMeshProUGUI>();
        gameOverText.text = "You Win!";
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
        healer.transform.position = healer.startPosition;
        foreach (Ally ally in allies)
        {
            ally.transform.localPosition = ally.startPosition;
            ally.Start();
            //ally.health = ally.maxHealth; // reset health
        }
        boss.Start();
        //boss.health = boss.maxHealth;

        // reset sprite direction
        // reset canvas
        normalCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        // reset score
        scoreText.text = "Score: 0";
        score = 0;
        // reset obstacles
        // foreach (Transform eachChild in obstacles.transform)
        // {
        //     eachChild.transform.localPosition = eachChild.GetComponent<ObstacleMovement>().startPosition;
        // }
        

    }
}
