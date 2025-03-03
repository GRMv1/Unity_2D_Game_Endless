using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDWindow : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI TimerTxt;
    [SerializeField]
    private TMPro.TextMeshProUGUI ScoreTxt;
    [SerializeField]
    private TMPro.TextMeshProUGUI HitCountTxt;
    [SerializeField]
    private Image LoadingScreen;
    [SerializeField]
    private TMPro.TextMeshProUGUI objectiveText;
    [SerializeField]
    private Button button;

    private TimeSpan timePlaying;
    private float elapsedTime;

    public float GetElapsedTime
    {
        get
        {
            return elapsedTime;
        }
    }

    public static HUDWindow Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            GameManager.Instance.BackToMainMenu();
        });

        TimerTxt.text = "Time: 00:00";
        ScoreTxt.text = "Score: 0";
        HitCountTxt.text = "Hit Count: 0";

        StartTimer();
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        StartCoroutine(UpdateTimer());
    }

    public void UpdateScore(int value)
    {
        ScoreTxt.text = "Score: " + value.ToString();
    }

    public void UpdateHitCount(int value)
    {
        HitCountTxt.text = "Hit Count: " + value.ToString();
    }

    private IEnumerator UpdateTimer()
    {
        while(true)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("ss'.'ff");
            TimerTxt.text = timePlayingStr;
            yield return null;
        }
    }

    public void ActivateLoadingScreen()
    {
        LoadingScreen.gameObject.SetActive(true);
    }
    public void DeactivateLoadingScreen()
    {
        LoadingScreen.gameObject.SetActive(false);
    }
    public void FadeOutLoadingScreen()
    {
        StartCoroutine(FadeOutLoadingScreenCoroutine());
    }
    public void FadeInLoadingScreen()
    {
        StartCoroutine(FadeInLoadingScreenCoroutine());
    }

    private IEnumerator FadeOutLoadingScreenCoroutine()
    {
        ActivateLoadingScreen();
        Color originalColor = LoadingScreen.color;
        for (float t = 0.0f; t < 2; t += Time.unscaledDeltaTime)
        {
            float normalizedTime = t / 2;
            LoadingScreen.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - normalizedTime);
            //additional fade for text with main objective
            objectiveText.color = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, 1 - normalizedTime);
            yield return null;
        }
        LoadingScreen.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        DeactivateLoadingScreen();
    }
    private IEnumerator FadeInLoadingScreenCoroutine()
    {
        ActivateLoadingScreen();
        Color originalColor = LoadingScreen.color;
        for (float t = 0.0f; t < 2; t += Time.unscaledDeltaTime)
        {
            float normalizedTime = t / 2;
            LoadingScreen.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0 + normalizedTime);
            yield return null;
        }
        LoadingScreen.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }
}
