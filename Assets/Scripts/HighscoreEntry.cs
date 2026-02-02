[System.Serializable]
public class HighscoreEntry
{
    public string name;
    public int score;

    public HighscoreEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
