using System.IO;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;

    private string path;
    private GameProgress gameProgress;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        path = Path.Combine(Application.persistentDataPath, "gameProgress.json");
        Debug.Log("Progress file path: " + path);
        LoadProgress();
    }

    private void LoadProgress()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            gameProgress = JsonUtility.FromJson<GameProgress>(json);
        }
        else
        {
            gameProgress = new GameProgress();
            SaveProgress();
        }
    }

    public void MarkLevelComplete(int levelNumber)
    {
        if (!gameProgress.completedLevels.Contains(levelNumber))
        {
            gameProgress.completedLevels.Add(levelNumber);
            SaveProgress();
        }
    }

    public bool IsLevelComplete(int levelNumber)
    {
        return levelNumber == 0 || levelNumber == 1 || gameProgress.completedLevels.Contains(levelNumber - 1);
    }

    private void SaveProgress()
    {
        string json = JsonUtility.ToJson(gameProgress, true);
        File.WriteAllText(path, json);
    }
}
