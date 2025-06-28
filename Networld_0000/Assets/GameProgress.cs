using System.Collections.Generic;

[System.Serializable]
public class LevelProgress
{
    public int levelNumber;
    public int highScore;
    public int stars;
}

[System.Serializable]
public class GameProgress
{
    public List<LevelProgress> completedLevels = new List<LevelProgress>();
}
