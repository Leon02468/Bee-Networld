using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
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
        ipPool = GeneratBeeIPs(5);
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
            //results[i] = $"{Random.Range(1, 255)}.{Random.Range(0, 256)}.{Random.Range(0, 256)}.{Random.Range(1, 255)}";
            //Testting with simple IPs
            results[i] = $"{Random.Range(1, 10)}.{Random.Range(0, 10)}.{Random.Range(0, 10)}.{Random.Range(1, 10)}";
        }
        return results;
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
