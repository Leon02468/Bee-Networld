using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public int level;
    public TextMeshProUGUI LevelText;
    
    void Awake()
    {
        ProgressManager.Instance.IsLevelComplete(level);
    }

    void Start()
    {
        if (level <= 0)
            LevelText.text = "DEMO";
        else
            LevelText.text = level.ToString(); // Set the text to the level number

        SetUI();
    }

    public void OpenScene()
    {
        if (!ProgressManager.Instance.IsLevelComplete(level) && level > 1)
        {
            // Send to Hung: Replace this warning with a simple UI popup
            Debug.LogWarning("You cannot access this level yet. Please click to message to see the comment above it.");
            return;
        }
        GameManager.Instance.selectedLevel = level; // Set the selected level in GameManager
        SceneManager.LoadScene("Level " + level.ToString()); // Load the LevelSelector scene
    }

    private void SetUI()
    {
        if (ProgressManager.Instance.IsLevelComplete(level) && level > 1)
        {
            GetComponent<Image>().color = new Color(1f, 0.6108978f, 0.1641509f);

        }
        else if (!ProgressManager.Instance.IsLevelComplete(level) || level > 1)
        {
            GetComponent<Image>().color = Color.grey;
        }
    }
}
