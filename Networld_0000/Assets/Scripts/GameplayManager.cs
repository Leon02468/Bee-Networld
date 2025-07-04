using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class GameplayManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]private TextMeshProUGUI clockText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private BeeManager beeManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private EndGameUIManager endGameUIManager;
    [SerializeField] private IpDisplay playerText;

    private int levelNumber;
    private string startingIP;
    private float timeLimit; //default time limit
    private float timer; //play time left
    private int minScore;
    private int middleScore;
    private int maxScore;
    private bool gameRunning;
    private string[] defaultIpPool; //default IP pool for bees
    private string[] ipPool; //current IP pool for bees

    void Start()
    {
        TextAsset levelJson = GameManager.Instance.GetLevelJson();
        if (levelJson != null)
        {
            LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
            levelNumber = levelData.levelNumber;
            startingIP = levelData.initialPlayerIP;
            timeLimit = levelData.timeLimit;
            minScore = levelData.minScore;
            middleScore = levelData.middleScore;
            maxScore = levelData.maxScore;
            scoreManager.SetMaxValue(levelData.maxScore);
            defaultIpPool = BuildIPPoolFromLevel(levelData);
            levelText.text = "LEVEL " + levelData.levelNumber;
        }
        else
        {
            levelNumber = 0;
            startingIP = "0.0.0.0";
            timeLimit = 70f;
            minScore = 9999;
            middleScore = 9999;
            maxScore = 9999;
            scoreManager.SetMaxValue(100);
            defaultIpPool = GeneratBeeIPs(5);
            levelText.text = "DEMO";
        }

        StartGame();
    }

    void Update()
    {
        if (!gameRunning) return;

        // Update timer
        timer -= Time.deltaTime;
        if (timer <= 0f || beeManager.isOutOfBees())
        {
            if(timer < 0f)
                timer = 0f;
            EndGame();
        }

        // Display time
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        clockText.text = seconds >= 10 ? $"0{minutes}:{seconds}" : $"0{minutes}:0{seconds}";

        // TAB KEY to switch bee
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            beeManager.CycleBee();
        }

        //// Check for Enter key and validate IP
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            CheckAndSendBee();
            Debug.Log("Score: " + scoreManager.GetCurrentScore());
        }
    }

    public void StartGame()
    {
        playerText.SetInitialIP(startingIP);
        timer = timeLimit;
        ipPool = new string[defaultIpPool.Length];
        defaultIpPool.CopyTo(ipPool, 0);
        ShuffleIpQueue(ipPool);
        gameRunning = true;
        beeManager.InitBeeWave(ipPool);
        beeManager.StartBeeSequence();
    }
    private void EndGame()
    {
        int currentScore = scoreManager.GetCurrentScore();
        bool isCompleted = currentScore > minScore ? true : false;
        int stars = CalculateStars();
        gameRunning = false;
        endGameUIManager.ShowEndGamePanel(
            isCompleted,
            levelNumber,
            currentScore,
            stars,
            (int)timer,
            (int)(timeLimit - timer)
        );
    }

    //Send to Hung: Add one more reset button for this method
    public void ResetGame()
    {
        endGameUIManager.SetToFalse();
        gameRunning = false;
        timer = timeLimit;
        scoreManager.ResetScore();
        beeManager.ClearAllBees();
        StartGame();
    }

    private string[] GeneratBeeIPs(int count)
    {
        string[] results = new string[count];
        for (int i = 0; i < count; i++)
        {
            results[i] = $"{Random.Range(1, 10)}.{Random.Range(0, 10)}.{Random.Range(0, 10)}.{Random.Range(1, 10)}";
        }
        return results;
    }

    private string[] BuildIPPoolFromLevel(LevelData level)
    {
        List<string> ipPool = new List<string>();

        foreach (var entry in level.beeQueue)
        {
            for (int i = 0; i < entry.count; i++)
            {
                ipPool.Add(entry.ip);
            }
        }

        return ipPool.ToArray();
    }

    private void ShuffleIpQueue(string[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    private void CheckAndSendBee()
    {
        if (beeManager.CurrentBee == null)
        {
            Debug.LogWarning("No active bee to check!"); return;
        }

        string playerIP = playerText.GetCurrentIP();
        string beeIP = beeManager.CurrentBee.targetIP;

        if (playerIP == beeIP)
        {
            scoreManager.AddScore(10);
        }

        beeManager.RemoveAndReplaceCurrentBee();
    }

    private int CalculateStars()
    {
        int score = scoreManager.GetCurrentScore();

        if (score >= maxScore) return 3;
        else if (score >= middleScore) return 2;
        else if (score >= minScore) return 1;
        else return 0;
    }
}
