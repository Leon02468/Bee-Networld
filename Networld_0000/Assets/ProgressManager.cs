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

    public int GetHighScore(int levelNumber)
    {
        return GetOrCreateLevelProgress(levelNumber).highScore;
    }

    public int GetStars(int levelNumber)
    {
        return GetOrCreateLevelProgress(levelNumber).stars;
    }

    public void SetScoreAndStars(int levelNumber, int score, int stars)
    {
        LevelProgress levelProgress = GetOrCreateLevelProgress(levelNumber);
        levelProgress.highScore = Mathf.Max(levelProgress.highScore, score);
        levelProgress.stars = Mathf.Max(levelProgress.stars, stars);
        SaveProgress();
    }

    private LevelProgress GetOrCreateLevelProgress(int levelNumber)
    {
        LevelProgress levelProgress = gameProgress.completedLevels.Find(l => l.levelNumber == levelNumber);

        if (levelProgress == null)
        {
            levelProgress = new LevelProgress { levelNumber = levelNumber };
            gameProgress.completedLevels.Add(levelProgress);
        }

        return levelProgress;
    }

    public void MarkLevelComplete(int levelNumber)
    {
        LevelProgress levelProgress = GetOrCreateLevelProgress(levelNumber);

        if (!gameProgress.completedLevels.Contains(levelProgress))
        {
            gameProgress.completedLevels.Add(levelProgress);
            SaveProgress();
        }
    }

    public bool IsLevelComplete(int levelNumber)
    {
        LevelProgress levelProgress = gameProgress.completedLevels.Find(l => l.levelNumber == levelNumber - 1);
        return levelNumber == 0 || levelNumber == 1 || gameProgress.completedLevels.Contains(levelProgress);
    }

    private void SaveProgress()
    {
        string json = JsonUtility.ToJson(gameProgress, true);
        File.WriteAllText(path, json);
    }
}
