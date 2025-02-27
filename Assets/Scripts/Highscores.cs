using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscores
{
    public List<ScoreElementSaveData> scoreElemnentList;

    [System.Serializable]
    public class ScoreElementSaveData
    {
        public int Time;
        public int Score;
        public int HitCount;
    }
}
