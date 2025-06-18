using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Slider scoreProgressBar;

    private int maxScore = 100;
    private int currentScore = 0;

    public void AddScore(int value)
    {
        currentScore += value;
        currentScore = Mathf.Min(currentScore, maxScore);

        scoreProgressBar.value = currentScore;
    }

    public void ResetScore()
    {
        currentScore = 0;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
