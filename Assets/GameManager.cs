using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ==================================================
    //              --- Snake-score ---
    // ==================================================
    public int score = 0;

    public TextMeshProUGUI scoreText;

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    public TextMeshProUGUI finalScoreText;

    public TextMeshProUGUI highscoreText;


    public TMP_InputField nameInput;
    public GameObject submitButton;

    private HighscoreManager highscoreManager;


    private void UpdateHighscoreUI()
    {
        if (highscoreText == null)
        {
            Debug.LogError("HighscoreText is not assigned!");
            return;
        }

        highscoreText.text = "TOP 10\n\n";

        int rank = 1;
        foreach (var entry in highscoreManager.highscores)
        {
            highscoreText.text += $"{rank}. {entry.name} - {entry.score}\n";
            rank++;
        }
    }


    private void Awake()
    {
        highscoreManager = FindAnyObjectByType<HighscoreManager>();

        if (highscoreManager == null)
        {
            Debug.LogError("HighscoreManager not found in scene!");
        }
    }


    // ==================================================
    //     --- Pause-, Quit-, og Game Over-panel ---
    // ==================================================
    public GameObject pausePanel;
    public GameObject confirmQuitPanel;

    public GameObject gameOverPanel;

    private bool isPaused = false;

    private bool isGameOver = false;


    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Return))
        {
            RestartGame();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        confirmQuitPanel.SetActive(false);
    }

    public void AskQuit()
    {
        pausePanel.SetActive(false);
        confirmQuitPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void GameOver()
    {
        if (gameOverPanel == null)
        {
            Debug.LogError("GameOverPanel is not assigned!");
            return;
        }

        isGameOver = true;
        Time.timeScale = 0f;

        nameInput.gameObject.SetActive(false);
        submitButton.SetActive(false);

        finalScoreText.text = "Final score: " + score;
        gameOverPanel.SetActive(true);

        // VIS ALLTID TOP-10:
        UpdateHighscoreUI();


        // Highscore-sjekk (kommer neste steg):
        if (highscoreManager != null && highscoreManager.IsHighscore(score))
        {
            nameInput.gameObject.SetActive(true);
            submitButton.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        gameOverPanel.SetActive(false);

        score = 0;
        scoreText.text = "Score: 0";

        // Restart sceen:
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }


    public void SubmitHighscore()
    {
        if (highscoreManager == null)
        {
            Debug.LogError("HighscoreManager is missing!");
            return;
        }

        string playerName = nameInput.text;

        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Anonymous";
        }

        highscoreManager.AddHighscore(playerName, score);

        // Skjul input etter lagring
        nameInput.gameObject.SetActive(false);
        submitButton.SetActive(false);


        // OPPDATER VISNINGEN
        UpdateHighscoreUI();
    }
}
