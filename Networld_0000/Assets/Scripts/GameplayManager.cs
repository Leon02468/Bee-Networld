using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using NUnit.Framework;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]private TextMeshProUGUI clockText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private BeeManager beeManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private IpDisplay playerText;

    private string startingIP;
    private float timeLimit; //default time limit
    private float timer; //play time left
    private bool gameRunning;
    private string[] defaultIpPool; //default IP pool for bees
    private string[] ipPool; //current IP pool for bees

    void Start()
    {
        TextAsset levelJson = GameManager.Instance.GetLevelJson();
        if (levelJson != null)
        {
            LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
            startingIP = levelData.initialPlayerIP;
            timeLimit = levelData.timeLimit;
            scoreManager.SetMaxValue(levelData.maxScore);
            defaultIpPool = BuildIPPoolFromLevel(levelData);
            levelText.text = "LEVEL " + levelData.levelNumber;
        }
        else
        {
            startingIP = "0.0.0.0";
            timeLimit = 70f;
            scoreManager.SetMaxValue(100);
            defaultIpPool = GeneratBeeIPs(5);
            levelText.text = "DEMO";
        }

        StartGame();
    }

    void Update()
    {
        if (!gameRunning) return;

        // TAB KEY to switch bee
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            beeManager.CycleBee();
        }

        // Update timer
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;
            EndGame();
        }

        // Display time
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        clockText.text = seconds >= 10 ? $"0{minutes}:{seconds}" : $"0{minutes}:0{seconds}";

        //// Check for Enter key and validate IP
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            CheckAndSendBee();
            Debug.Log("Score: " + scoreManager.GetCurrentScore());
        }
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
            for(int i = 0; i < entry.count; i++)
            {
                ipPool.Add(entry.ip);
            }
        }

        return ipPool.ToArray();
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

    public void StartGame()
    {
        playerText.SetInitialIP(startingIP);
        timer = timeLimit;
        ipPool = new string[defaultIpPool.Length];
        ipPool = defaultIpPool;
        gameRunning = true;
        beeManager.InitBeeWave(ipPool);
        beeManager.StartBeeSequence();
    }
    private void EndGame()
    {
        gameRunning = false;
        Debug.Log("Game Over. Final Score: " + scoreManager.GetCurrentScore());
    }

    //Send to Hung: Add one more reset button for this method
    public void ResetGame()
    {
        gameRunning = false;
        timer = timeLimit;
        scoreManager.ResetScore();
        beeManager.ClearAllBees();
        StartGame();
    }

    // Minh part, don't touch this, can add music or UI to other functions and Minh
    // will use them to these parts
    private void Victory()
    {
        // Add victory logic
        // Mark the game as completed
    }

    private void Lose()
    {
        // Add lose logic
    }
}
