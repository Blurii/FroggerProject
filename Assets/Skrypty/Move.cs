using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 5f;
    public Vector3 startPosition;
    public int lives = 3;
    public int maxScore = 100;
    public int moveCount = 0;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isGameOver = false;
    private bool isWin = false;
    private bool canMove = true;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject pausePanel;
    public InputField playerNameInput;
    public Text winMessageText;
    public Text scoreText;
    public Text movesText;
    public int menuSceneIndex = 0;
    public int lifeBonus = 50;
    public Image[] lifeImages;
    public Button saveScoreButton;
    public Button returnToMenuButton;
    public Button resumeButton;
    void Start()
    {
        Time.timeScale = 1f;
        startPosition = transform.position;
        targetPosition = startPosition;

        Camera camera = Camera.main;
        if (camera != null)
        {
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;
            Vector3 cameraPosition = camera.transform.position;
            minBounds = new Vector2(cameraPosition.x - halfWidth, cameraPosition.y - halfHeight);
            maxBounds = new Vector2(cameraPosition.x + halfWidth, cameraPosition.y + halfHeight);
        }
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        UpdateHUD();
    }
    void Update()
    {
        if (isGameOver || isWin)
        {
            canMove = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver && !isWin)
        {
            TogglePause();
        }
        if (!isMoving && lives > 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                Move(Vector3.up);
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                Move(Vector3.down);
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                Move(Vector3.left);
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                Move(Vector3.right);
        }
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    void Move(Vector3 direction)
    {
        Vector3 newPosition = targetPosition + direction * moveDistance;
        if (newPosition.x >= minBounds.x && newPosition.x <= maxBounds.x &&
            newPosition.y >= minBounds.y && newPosition.y <= maxBounds.y)
        {
            targetPosition = newPosition;
            isMoving = true;
            moveCount++;
            RotateFrog(direction);
            UpdateHUD();
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("WinZone"))
        {
            WinGame();
        }
        if (collider.CompareTag("Vehicle"))
        {
            LoseLife();
        }
    }

    void LoseLife()
    {
        lives--;

        if (lives >= 0 && lives < lifeImages.Length)
        {
            lifeImages[lives].enabled = false;
        }

        if (lives > 0)
        {
            transform.position = startPosition;
            targetPosition = startPosition;
        }
        else
        {
            GameOver();
        }

        UpdateHUD();
    }
    void GameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    void RotateFrog(Vector3 direction)
    {
        if (direction == Vector3.up)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction == Vector3.down)
            transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (direction == Vector3.left)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (direction == Vector3.right)
            transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    void WinGame()
    {
        int baseScore = Mathf.Max(maxScore - moveCount, 0);
        int lifeScore = lives * lifeBonus;
        int finalScore = baseScore + lifeScore;
        isGameOver = true;
        isWin = true;
        canMove = false;
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            winMessageText.text = $"Wygra³eœ! Twój wynik to: {finalScore}, aby zapisaæ wpisz swoj¹ nazwê i kliknij zapisz";
        }

        Time.timeScale = 0f;
    }

    public void SaveScore()
    {
        Time.timeScale = 0f;
        if (playerNameInput != null)
        {
            string playerName = playerNameInput.text;
            int finalScore = Mathf.Max(maxScore - moveCount + (lives * lifeBonus), 0);
            if (!string.IsNullOrEmpty(playerName))
            {
                SaveHighScore(playerName, finalScore);
                ReturnToMenu();
            }
            else
            {
                SaveHighScore("Anonim", finalScore);
                ReturnToMenu();
            }
        }
        
    }

    void SaveHighScore(string playerName, int score)
    {
        string highScores = PlayerPrefs.GetString("HighScores", "");
        string newEntry = playerName + ":" + score;
        if (!string.IsNullOrEmpty(highScores))
        {
            highScores += "|" + newEntry;
        }
        else
        {
            highScores = newEntry;
        }
        PlayerPrefs.SetString("HighScores", highScores);
        PlayerPrefs.Save();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        canMove = true;
        SceneManager.LoadScene(menuSceneIndex);
    }

    void UpdateHUD()
    {
        if (scoreText != null)
        {
            int currentScore = Mathf.Max(maxScore - moveCount, 0) + (lives * lifeBonus);
            scoreText.text = "Wynik: " + currentScore;
        }
        if (movesText != null)
            movesText.text = "Ruchy: " + moveCount;
    }
    void TogglePause()
    {
        if (pausePanel != null)
        {
            bool isPaused = Time.timeScale == 0f;
            pausePanel.SetActive(!isPaused);
            Time.timeScale = isPaused ? 1f : 0f;
        }
    }
    public void ResumeGame()
    {
        TogglePause();
    }
}
