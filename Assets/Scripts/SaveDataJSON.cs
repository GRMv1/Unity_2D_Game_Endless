using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataJSON : MonoBehaviour
{
    public static SaveDataJSON Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    public void SaveSettingsData()
    {
        string json = JsonUtility.ToJson(GameManager.Instance.Settings);

        using(StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "SettingsSaveData.json"))
        {
            writer.Write(json);
        }
    }

    public void SaveHighscoreTableData()
    {
        string json = JsonUtility.ToJson(GameManager.Instance.Highscores);

        using (StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "HighscoreTableSaveData.json"))
        {
            writer.Write(json);
        }
    }

    public Settings LoadSettingsData()
    {
        string json = string.Empty;
        string filePath = Application.dataPath + Path.AltDirectorySeparatorChar + "SettingsSaveData.json";

        if(File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                json = reader.ReadToEnd();
            }

            Settings settingsData = JsonUtility.FromJson<Settings>(json);

            return settingsData;
        }
        else
        {
            return new Settings { Volume = 1.0f, IsDefaultFont = true };
        }
    }

    public Highscores LoadHighscoreTableData()
    {
        string json = string.Empty;
        string filePath = Application.dataPath + Path.AltDirectorySeparatorChar + "HighscoreTableSaveData.json";

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                json = reader.ReadToEnd();
            }

            Highscores highscoreData = JsonUtility.FromJson<Highscores>(json);

            return highscoreData;
        }
        else
        {
            return new Highscores { scoreElemnentList = new List<Highscores.ScoreElementSaveData>() };
        }
    }
}
