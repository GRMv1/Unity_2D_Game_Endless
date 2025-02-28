using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    [SerializeField]
    private Transform ScoreContainer;
    [SerializeField]
    private ScoreElement ScoreElementTemplate;

    private List<ScoreElement> scoreElementsList;
    private Vector3 offScreenPosition;
    private Vector3 onScreenPosition;
    private RectTransform rt;
    private float duration = 1.0f;
    private Coroutine coroutine;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        onScreenPosition = new Vector3(Screen.width - 5, transform.position.y, 0);
        offScreenPosition = new Vector3(Screen.width + rt.rect.width, transform.position.y, 0);
        transform.position = offScreenPosition;
    }

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
    private IEnumerator ShowHideCoroutine(bool isVisible)
    {
        float elapsedTime = 0.0f;
        Vector3 initialPosition = transform.position;
        if (isVisible)
        {
            while(elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                transform.position = Vector3.Lerp(initialPosition, offScreenPosition, elapsedTime / duration);
                yield return null;
            }
            transform.position = offScreenPosition;
        }
        else
        {
            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                transform.position = Vector3.Lerp(initialPosition, onScreenPosition, elapsedTime / duration);
                yield return null;
            }
            transform.position = onScreenPosition;
        }
    }

    public void ShowHideLeaderboard()
    {
        if(transform.position == offScreenPosition)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(ShowHideCoroutine(false));
        }
        else if(transform.position == onScreenPosition)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(ShowHideCoroutine(true));
        }
    }
}
