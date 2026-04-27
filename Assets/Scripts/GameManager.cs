using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreText;
    
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gameOverScoreText;
    public GameObject gameOverPanel;
    public TMP_InputField nameInputField;
    public Leaderboard leaderboard;

    public UnityEngine.UI.Button submitScoreButton;
    public TextMeshProUGUI submitStatusText;

    private int score = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "Best: " + highScore;
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void UpdateHealth(int health)
    {
        
    }

    public void UpdateWave(int wave)
    {
        waveText.text = "Wave " + wave;
    }

    public void SubmitPlayerScore()
    {
        string playerName = nameInputField.text;
        if (string.IsNullOrEmpty(playerName))
            playerName = "Anonymous";

        submitScoreButton.gameObject.SetActive(false);
        submitStatusText.text = "Sending...";

        leaderboard.SubmitScore(playerName, score, (success) =>
        {
            if (success)
            {
                submitStatusText.text = "Score submitted!";
            }
            else
            {
                submitScoreButton.gameObject.SetActive(true);
                submitStatusText.text = "Failed. Try again.";
            }
        });
    }

    public void GameOver()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreText.text = "Best: " + score;
        }

        if (gameOverScoreText != null)
            gameOverScoreText.text = "Score: " + score;

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}