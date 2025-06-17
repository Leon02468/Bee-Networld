using UnityEngine;
using UnityEngine.SceneManagement; // Needed for exiting the game in builds

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel; // Assign the Menu panel in the Inspector
    private bool isPaused = false;

    void Start()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        if (menuPanel != null)
            menuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (menuPanel != null)
            menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Assign this to the Resume button's OnClick event
    public void OnResumeButton()
    {
        ResumeGame();
    }

    // Assign this to the Exit button's OnClick event
    public void OnExitButton()
    {
        //#if UNITY_EDITOR
        //        UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //        Application.Quit();
        //#endif

        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }
}