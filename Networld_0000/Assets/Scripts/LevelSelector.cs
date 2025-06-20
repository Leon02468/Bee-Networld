using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public int level;
    public TextMeshProUGUI LevelText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LevelText.text = level.ToString(); // Set the text to the level number
    }

    // Update is called once per frame
    public void OpenScene()
    {
        SceneManager.LoadScene("Level " + level.ToString()); // Load the LevelSelector scene
    }
}
