using UnityEngine;

public class SceneMusicSetter : MonoBehaviour
{
    public string levelName; // Set this in the Inspector

    void Start()
    {
        var musicManager = FindFirstObjectByType<MusicManager>();
        if (musicManager != null && !string.IsNullOrEmpty(levelName))
        {
            musicManager.PlayMusicForLevel(levelName);
        }
    }
}