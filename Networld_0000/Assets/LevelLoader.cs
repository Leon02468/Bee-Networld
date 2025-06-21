using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelJson;

    public LevelData LoadLevel()
    {
        if (levelJson == null)
        {
            Debug.LogError("Level JSON not assigned!");
            return null;
        }

        return JsonUtility.FromJson<LevelData>(levelJson.text);
    }

    //public TextAsset levelJson;

    //public static LevelData LoadCurrentLevel()
    //{
    //    if (GameManager.Instance == null)
    //    {
    //        Debug.LogError("GameManager.instance is null in LevelLoader!");
    //        return null;
    //    }

    //    TextAsset levelJson = GameManager.Instance.GetLevelJson();
    //    if (levelJson == null)
    //    {
    //        Debug.LogWarning("No level JSON found for current level.");
    //        return null;
    //    }

    //    LevelData data = JsonUtility.FromJson<LevelData>(levelJson.text);
    //    if (data == null)
    //        Debug.LogError("Failed to parse LevelData from JSON.");

    //    return data;
    //}
}
