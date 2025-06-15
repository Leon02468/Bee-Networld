using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI clockText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI beeIPText;
    public Button startButton;
    public Button replayButton;

    private float timeLeft = 30f;
    private bool gameRunning = false;
    private int score = 0;

    void Start()
    {
        clockText.text = "Time: 30s";
        scoreText.text = "Score: 0";
        beeIPText.text = "0.0.0.0";

        startButton.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameRunning) return;

        // Update timer
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            EndGame();
        }

        // Display time
        int seconds = Mathf.CeilToInt(timeLeft);
        clockText.text = "Time: " + seconds + "s";

        // Check for Enter key and validate IP
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            TrySubmit();
        }
    }

    public void StartGame()
    {
        score = 0;
        timeLeft = 30f;
        gameRunning = true;

        GenerateNewBeeIP();
        startButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);

        Debug.Log("Game Started!");
    }
    public void ReplayGame()
    {
        StartGame();
    }

    void TrySubmit()
    {
        if (!gameRunning) return;

        string playerIP = playerText.text.Trim();
        string beeIP = beeIPText.text.Trim();

        if (playerIP == beeIP)
        {
            score += 10;
            scoreText.text = "Score: " + score;
            Debug.Log("Correct IP! Score: " + score);
            GenerateNewBeeIP(); // Generate a new IP for the next round
        }
        else
        {
            Debug.Log("Wrong IP!");
        }
    }

    void GenerateNewBeeIP()
    {
        string ip = $"{Random.Range(0, 256)}.{Random.Range(0, 256)}.{Random.Range(0, 256)}.{Random.Range(0, 256)}";
        beeIPText.text = ip;
        Debug.Log("New Bee IP: " + ip);
    }

    void EndGame()
    {
        gameRunning = false;
        replayButton.gameObject.SetActive(true);
        Debug.Log("Game Over. Final Score: " + score);
    }
}
