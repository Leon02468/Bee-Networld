using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Slider scoreProgressBar;

    private int currentScore = 0;

    public void AddScore(int value)
    {
        currentScore += value;

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

    public void SetMaxValue(int value)
    {
        scoreProgressBar.maxValue = value;
    }
}
