using UnityEngine;

public class EndGameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private Sprite[] endGameImages;
    [SerializeField] private UnityEngine.UI.Image endGameImage;
    [SerializeField] private Sprite[] stars;
    [SerializeField] private GameObject[] starObjects;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject replayButton;
    [SerializeField] private TMPro.TextMeshProUGUI finalText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI totalScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI highScoreText;

    void Start()
    {
        SetToFalse();
    }

    public void SetToFalse()
    {
        endGamePanel.SetActive(false);
        nextButton.SetActive(false);
        replayButton.SetActive(false);
    }

    public void ShowEndGamePanel(
        bool isCompleted, int levelNumber, int score,
        int stars, int timeRemain, int timeFinsh)
    {
        int minutes = Mathf.FloorToInt(timeFinsh / 60f);
        int seconds = Mathf.FloorToInt(timeFinsh % 60f);

        ProgressManager.Instance.SetScoreAndStars(levelNumber, score, stars);

        endGamePanel.SetActive(true);
        timeText.text = seconds >= 10 ? $"0{minutes}:{seconds}" : $"0{minutes}:0{seconds}";
        scoreText.text = score.ToString();
        totalScoreText.text = TotalScoreCalculation(score, timeRemain).ToString();

        if (isCompleted)
        {
            GameCompleted();
        }
        else
        {
            GameFailed();
        }

        SetStars(stars);
        highScoreText.text = ProgressManager.Instance.GetHighScore(levelNumber).ToString();
    }

    private int TotalScoreCalculation(int score, int timeRemain)
    {
        return score + timeRemain;
    }

    private void GameCompleted()
    {
        endGameImage.sprite = endGameImages[1];
        finalText.text = "COMPLETED";
        nextButton.SetActive(true);
    }

    private void GameFailed()
    {
        endGameImage.sprite = endGameImages[0];
        finalText.text = "FAILED";
        replayButton.SetActive(true);
    }

    private void SetStars(int starsCount)
    {
        for (int i = 0; i < starObjects.Length; i++)
        {
            if (i < starsCount)
            {
                starObjects[i].GetComponent<UnityEngine.UI.Image>().sprite = stars[1];
            }
            else
            {
                starObjects[i].GetComponent<UnityEngine.UI.Image>().sprite = stars[0];
            }
        }
    }
}
