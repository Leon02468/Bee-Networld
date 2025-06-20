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
}
