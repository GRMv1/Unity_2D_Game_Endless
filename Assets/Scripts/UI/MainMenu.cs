using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public HighscoreTable HighscoreTable;
    public Image LoadingScreen;

    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private Toggle defaultFontToggle;

    public static MainMenu Instance
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
        LoadingScreen.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void ChangeVolume()
    {
        GameManager.Instance.ChangeVolume(volumeSlider.value);
    }

    public void SwapFont()
    {
        GameManager.Instance.SwapFont(defaultFontToggle.isOn);
    }

    public void SetupMenu(float value, bool IsDefaultFont)
    {
        volumeSlider.value = value;
        defaultFontToggle.isOn = IsDefaultFont;
    }

    private void ActivateLoadingScreen()
    {
        LoadingScreen.gameObject.SetActive(true);
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
            yield return null;
        }
        LoadingScreen.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
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
