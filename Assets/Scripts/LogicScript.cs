using TMPro;
using UnityEngine;

public class LogicScript : MonoBehaviour
{

    public TextMeshProUGUI scoreText;

    [System.NonSerialized]
    public int score = 0; // we don't want this to show up in the inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score.ToString();
    }
}
