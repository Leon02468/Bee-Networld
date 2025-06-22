using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeeEntry
{
    public string ip;
    public int count;
}

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public int timeLimit;
    public int maxScore;
    public List<BeeEntry> beeQueue;
    public string initialPlayerIP;
}