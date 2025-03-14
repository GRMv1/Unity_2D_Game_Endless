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
    private Coroutine coroutine;

    private static float DURATION = 1.0f;

    private void Start()
    {
        var canvasScaler = GetComponentInParent<CanvasScaler>();
        Vector2 referenceResolution = canvasScaler.referenceResolution;
        Vector2 currentResolution = new Vector2(Screen.width, Screen.height);
        var scaleFactor = currentResolution.y / referenceResolution.y;
        rt = GetComponent<RectTransform>();
        onScreenPosition = new Vector3(Screen.width - 5, transform.position.y, 0);
        offScreenPosition = new Vector3(Screen.width + rt.rect.width * scaleFactor, transform.position.y, 0);
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

    private void CreateScoreElement(int number, float time, int score, int hitCount)
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
            while(elapsedTime < DURATION)
            {
                elapsedTime += Time.unscaledDeltaTime;
                transform.position = Vector3.Lerp(initialPosition, offScreenPosition, elapsedTime / DURATION);
                yield return null;
            }
            transform.position = offScreenPosition;
        }
        else
        {
            while (elapsedTime < DURATION)
            {
                elapsedTime += Time.unscaledDeltaTime;
                transform.position = Vector3.Lerp(initialPosition, onScreenPosition, elapsedTime / DURATION);
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
