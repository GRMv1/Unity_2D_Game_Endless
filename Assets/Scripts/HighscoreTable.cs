using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreTable : MonoBehaviour
{
    public Transform ScoreContainer;
    public ScoreElement ScoreElementTemplate;
    private List<ScoreElement> scoreElementsList;

    public void InitHighscoreTable(List<Highscores.ScoreElementSaveData> scoreList)
    {
        scoreElementsList = new List<ScoreElement>();

        if(scoreList != null)
        {
            scoreList.Sort((item1, item2) => item1.Time.CompareTo(item2.Time));

            for (int i = 0; i < scoreList.Count; i++)
            {
                int place = i + 1;
                CreateScoreElement(place, scoreList[i].Time, scoreList[i].Score, scoreList[i].HitCount);
            }
        }
    }

    private void CreateScoreElement(int number, int time, int score, int hitCount)
    {
        ScoreElement scoreElement = Instantiate(ScoreElementTemplate, ScoreContainer);
        scoreElement.SetValues(number, time, score, hitCount);
        scoreElementsList.Add(scoreElement);
    }
}
