using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public int selectedLevel;
    public TextAsset[] levelJsonFiles; // JSON file for level data

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
    }

    public TextAsset GetLevelJson()
    {
        if (selectedLevel - 1 >= 0)
            if (selectedLevel - 1 < levelJsonFiles.Length)
                return levelJsonFiles[selectedLevel - 1];
        return null;
    }
}
