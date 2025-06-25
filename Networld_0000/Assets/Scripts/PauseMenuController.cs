using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Menu UI")]
    public GameObject menuCanvas;      // Reference to the MenuCanvas (second layer)
    public GameObject resumeButton;
    public GameObject exitButton;
    public GameObject settingsButton;

    [Header("Music Settings UI")]
    public GameObject musicSettingsCanvas;
    public Button backButton;

    private void Start()
    {
        // Hide only the menu canvas at game start
        if (menuCanvas != null)
            menuCanvas.SetActive(false);

        // Ensure MusicSettings is hidden at start
        if (musicSettingsCanvas != null)
            musicSettingsCanvas.SetActive(false);

        if (backButton != null)
            backButton.onClick.AddListener(OnBackFromMusicSettings);
    }

    private void Update()
    {
        // Disable ESC key handling when music settings are open
        if (musicSettingsCanvas != null && musicSettingsCanvas.activeSelf)
            return;

        // ESC key toggles the menu canvas
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuCanvas != null && !menuCanvas.activeSelf)
            {
                // Show menu canvas and pause game
                menuCanvas.SetActive(true);
                Time.timeScale = 0f;
                if (musicSettingsCanvas != null) musicSettingsCanvas.SetActive(false);
            }
            else if (menuCanvas != null && menuCanvas.activeSelf)
            {
                // Hide menu canvas and resume game
                menuCanvas.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    public void OnResumeClick()
    {
        // Hide the menu canvas (second shell)
        if (menuCanvas != null)
            menuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnExitClick()
    {
        Time.timeScale = 1f; // Ensure game is unpaused before loading
        SceneManager.LoadSceneAsync(0); // Load the start menu scene (index 0)
    }

    public void OnSettingsClick()
    {
        // Hide only the buttons
        if (resumeButton != null) resumeButton.SetActive(false);
        if (exitButton != null) exitButton.SetActive(false);
        if (settingsButton != null) settingsButton.SetActive(false);

        // Show MusicSettings canvas
        if (musicSettingsCanvas != null) musicSettingsCanvas.SetActive(true);
    }

    public void OnBackFromMusicSettings()
    {
        // Show the buttons
        if (resumeButton != null) resumeButton.SetActive(true);
        if (exitButton != null) exitButton.SetActive(true);
        if (settingsButton != null) settingsButton.SetActive(true);

        // Hide MusicSettings canvas
        if (musicSettingsCanvas != null) musicSettingsCanvas.SetActive(false);
    }
}