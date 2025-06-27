using UnityEngine;

public class SceneMusicSetter : MonoBehaviour
{
    private string levelName; // Set this in the Inspector

    void Start()
    {
        if(GameManager.Instance != null)
            levelName = GameManager.Instance.GetCurrentLevelName();

        var musicManager = FindFirstObjectByType<MusicManager>();
        if (musicManager != null && !string.IsNullOrEmpty(levelName))
        {
            musicManager.PlayMusicForLevel(levelName);
        }
    }
}