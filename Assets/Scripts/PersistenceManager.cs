using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance { get; private set; }
    public List<HighScore> HighScores;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScores();
    }

    public void SaveHighScores()
    {
        SaveData data = new SaveData();
        data.HighScores = HighScores;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScores()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScores = data.HighScores;
        }
        else
        {
            HighScores = new List<HighScore>();
        }
    }

    [System.Serializable]
    class SaveData
    {
        public List<HighScore> HighScores;
    }

    [System.Serializable]
    public class HighScore
    {
        public string PlayerName;
        public int Score;
    }
}
