using System.Collections.Generic;
using UnityEngine;

// Lager et Top-10 highscore-system med navneinput

public class HighscoreManager : MonoBehaviour
{
    private const int MaxEntries = 10;
    private const string SaveKey = "HIGHSCORES";

    public List<HighscoreEntry> highscores = new();

    private void Awake()
    {
        Load();
    }

    public bool IsHighscore(int score)
    {
        if (highscores.Count < MaxEntries) return true;
        return score > highscores[^1].score;
    }

    public void AddHighscore(string name, int score)
    {
        highscores.Add(new HighscoreEntry(name, score));
        highscores.Sort((a, b) => b.score.CompareTo(a.score));

        if (highscores.Count > MaxEntries)
            highscores.RemoveAt(highscores.Count - 1);

        Save();
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(new HighscoreList(highscores));
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;

        string json = PlayerPrefs.GetString(SaveKey);
        highscores = JsonUtility.FromJson<HighscoreList>(json).entries;
    }

    [System.Serializable]
    private class HighscoreList
    {
        public List<HighscoreEntry> entries;
        public HighscoreList(List<HighscoreEntry> entries) => this.entries = entries;
    }
}
