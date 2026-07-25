using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    private PlayerMovement player;
    private Spawner spawner;
    private float score;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        spawner = FindObjectOfType<Spawner>();
        SetNewGame();
    }

    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
    }

    public void SetNewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (var obstacle in obstacles) Destroy(obstacle.gameObject);
        gameSpeed = initialGameSpeed;
        score = 0f;
        enabled = true;
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        UpdateHighScore();
    }

    public void GameOver() {
        gameSpeed = 0f;
        enabled = false;
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        UpdateHighScore();
    }

    private void UpdateHighScore() {
        float highScore = PlayerPrefs.GetFloat("highScore", 0);
        if (score > highScore) {
            highScore = score;
            PlayerPrefs.SetFloat("highScore", highScore);
        }
        highScoreText.text = Mathf.FloorToInt(highScore).ToString("D5");
    }
}