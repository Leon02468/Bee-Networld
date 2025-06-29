using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicSetter : MonoBehaviour
{
    private string levelName;

    void Start()
    {
        var musicManager = FindFirstObjectByType<MusicManager>();
        string sceneName = SceneManager.GetActiveScene().name;

        // Exception: If we're in the start menu scene, play start menu music
        if (sceneName == "Start Menu")
        {
            if (musicManager != null)
            {
                musicManager.PlayMusicForLevel("Start Menu");
            }
        }
        else if (GameManager.Instance != null)
        {
            levelName = GameManager.Instance.GetCurrentLevelName();
            if (musicManager != null && !string.IsNullOrEmpty(levelName))
            {
                musicManager.PlayMusicForLevel(levelName);
            }
        }
    }
}