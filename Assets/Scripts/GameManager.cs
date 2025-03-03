using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event Action<bool> OnSettingChange;

    private Settings settings;
    private Highscores highscores;
    private Highscores.ScoreElementSaveData currentScoreElement;

    private static int MAX_SCORE = 100;

    public Settings Settings
    {
        get => settings;
    }

    public Highscores Highscores
    {
        get => highscores;
    }

    void Start()
    {
        StartCoroutine(InitCoroutine());
    }

    private IEnumerator InitCoroutine()
    {
        settings = SaveDataJSON.Instance.LoadSettingsData();
        AudioListener.volume = settings.Volume;
        MainMenu.Instance.SetupMenu(settings.Volume, settings.IsDefaultFont);
        highscores = SaveDataJSON.Instance.LoadHighscoreTableData();
        MainMenu.Instance.HighscoreTable.InitHighscoreTable(highscores.scoreElemnentList);

        yield return new WaitForEndOfFrame();

        MainMenu.Instance.FadeOutLoadingScreen();
        OnSettingChange?.Invoke(settings.IsDefaultFont);
    }

    public static GameManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        Time.timeScale = 0;
        CanvasGroup canvasGroup = MainMenu.Instance.GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        currentScoreElement = new Highscores.ScoreElementSaveData();
        highscores.scoreElemnentList.Add(currentScoreElement);
        StartCoroutine(LoadScene());
    }
    private void StartNewRun()
    {
        Time.timeScale = 0;
        UpdateCurrentScoreElement();
        SaveGameData();
        currentScoreElement = new Highscores.ScoreElementSaveData();
        highscores.scoreElemnentList.Add(currentScoreElement);
        StartCoroutine(ReloadScene());

    }
    private IEnumerator LoadScene()
    {
        MainMenu.Instance.FadeInLoadingScreen();
        yield return new WaitForSecondsRealtime(2);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SwapFont(settings.IsDefaultFont);
        HUDWindow.Instance.FadeOutLoadingScreen();
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1;
    }

    private IEnumerator ReloadScene()
    {
        HUDWindow.Instance.FadeInLoadingScreen();
        yield return new WaitForSecondsRealtime(2);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SwapFont(settings.IsDefaultFont);
        HUDWindow.Instance.FadeOutLoadingScreen();
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }

    public void SaveGame()
    {
        SaveDataJSON.Instance.SaveSettingsData();
    }

    public void SaveGameData()
    {
        SaveDataJSON.Instance.SaveHighscoreTableData();
    }

    public void ChangeVolume(float value)
    {
        settings.Volume = value;
        AudioListener.volume = value;
        SaveDataJSON.Instance.SaveSettingsData();
    }

    public void SwapFont(bool isOn)
    {
        settings.IsDefaultFont = isOn;
        OnSettingChange?.Invoke(isOn);
        SaveDataJSON.Instance.SaveSettingsData();
    }

    public void SetSettings(float volume, bool isDefaultFont)
    {
        ChangeVolume(volume);
        SwapFont(isDefaultFont);
        MainMenu.Instance.SetupMenu(volume, isDefaultFont);
    }

    public void UpdateCurrentScoreElement()
    {
        float value = HUDWindow.Instance.GetElapsedTime;
        float roundedValue = Mathf.Round(value * 100f) / 100f;
        currentScoreElement.Time = roundedValue;
    }

    public void PlayerGotHit()
    {
        currentScoreElement.HitCount++;
        HUDWindow.Instance.UpdateHitCount(currentScoreElement.HitCount);
    }

    public void PlayerGotCoin(int value)
    {
        if(currentScoreElement.Score + value > MAX_SCORE)
        {
            currentScoreElement.Score = MAX_SCORE;
        }
        else
        {
            currentScoreElement.Score += value;
        }
        HUDWindow.Instance.UpdateScore(currentScoreElement.Score);
        if(currentScoreElement.Score >= MAX_SCORE)
        {
            StartNewRun();
        }
    }
}
