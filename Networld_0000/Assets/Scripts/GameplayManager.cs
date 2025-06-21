using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using NUnit.Framework;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour
{
    [Header("UI References")]
    //public TextMeshProUGUI clockText;

    [SerializeField] private BeeManager beeManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TextMeshProUGUI playerText;

    //private float timeLeft = 60f;
    //private bool gameRunning = false;
    private string[] ipPool;

    void Start()
    {
        //TextAsset levelJson = GameManager.instance.GetLevelJson();
        //if (levelJson != null)
        //{
        //    LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
        //    ipPool = BuildIPPoolFromLevel(levelData);
        //}
        //else
        //    ipPool = GeneratBeeIPs(5);
        //beeManager.InitBeeWave(ipPool);
        //beeManager.StartBeeSequence();

        //if (GameManager.instance == null)
        //{
        //    Debug.LogError("GameManager.instance is null!");
        //    return;
        //}

        //TextAsset levelJson = GameManager.instance.GetLevelJson();
        //if (levelJson != null)
        //{
        //    LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
        //    ipPool = BuildIPPoolFromLevel(levelData);
        //}
        //else
        //{
        //    Debug.LogWarning("levelJson is null, using generated IPs.");
        //    ipPool = GeneratBeeIPs(5);
        //}
        //beeManager.InitBeeWave(ipPool);
        //beeManager.StartBeeSequence();

        LevelData levelData = LevelLoader.LoadCurrentLevel();
        if (levelData != null && levelData.beeQueue != null && levelData.beeQueue.Count > 0)
        {
            ipPool = BuildIPPoolFromLevel(levelData);
        }
        else
        {
            Debug.LogWarning("LevelData or beeQueue is null/empty, using generated IPs.");
            ipPool = GeneratBeeIPs(5);
        }
        beeManager.InitBeeWave(ipPool);
        beeManager.StartBeeSequence();

    }

    void Update()
    {
        //if (!gameRunning) return;

        // TAB KEY to switch bee
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            beeManager.CycleBee();
        }

        //// Update timer
        //timeLeft -= Time.deltaTime;
        //if (timeLeft <= 0f)
        //{
        //    timeLeft = 0f;
        //    EndGame();
        //}

        //// Display time
        //int seconds = Mathf.CeilToInt(timeLeft);
        //clockText.text = "Time: " + seconds + "s";

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

        string playerIP = playerText.text.Trim();
        string beeIP = beeManager.CurrentBee.targetIP;

        if (playerIP == beeIP)
        {
            scoreManager.AddScore(10);
        }

        beeManager.RemoveAndReplaceCurrentBee();
    }

    //void EndGame()
    //{
    //    gameRunning = false;
    //    //replayButton.gameObject.SetActive(true);
    //    Debug.Log("Game Over. Final Score: " + score);
    //}
}
