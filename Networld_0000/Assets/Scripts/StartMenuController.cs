using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [Header("Main Menu UI")]
    public GameObject startButton;
    public GameObject exitButton;
    public GameObject settingsButton;

    [Header("Music Settings UI")]
    public GameObject musicSettingsCanvas; // Assign the MusicSettings Canvas here
    public Button backButton; // Assign the Back button from MusicSettings here

    void Start()
    {
        // Ensure MusicSettings is hidden at start
        if (musicSettingsCanvas != null)
            musicSettingsCanvas.SetActive(false);

        // Hook up the back button event
        if (backButton != null)
            backButton.onClick.AddListener(OnBackFromMusicSettings);
    }

    public void OnStartClick()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OnSettingsClick()
    {
        // Hide main menu buttons
        if (startButton != null) startButton.SetActive(false);
        if (exitButton != null) exitButton.SetActive(false);
        if (settingsButton != null) settingsButton.SetActive(false);

        // Show MusicSettings canvas
        if (musicSettingsCanvas != null) musicSettingsCanvas.SetActive(true);
    }

    public void OnBackFromMusicSettings()
    {
        // Show main menu buttons
        if (startButton != null) startButton.SetActive(true);
        if (exitButton != null) exitButton.SetActive(true);
        if (settingsButton != null) settingsButton.SetActive(true);

        // Hide MusicSettings canvas
        if (musicSettingsCanvas != null) musicSettingsCanvas.SetActive(false);
    }
}