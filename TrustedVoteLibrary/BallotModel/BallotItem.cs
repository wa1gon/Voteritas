namespace TrustedVoteLibrary;

public abstract class BallotItem
{
    public string Title { get; set; }
    public Dictionary<string, int> Options { get; private set; }

    protected BallotItem(string title, List<string> options)
    {
        Title = title;
        Options = new Dictionary<string, int>();
        foreach (var option in options)
        {
            Options[option] = 0;
        }
    }

    public abstract void RecordVote(List<string> selections);

    public void DisplayItem()
    {
        Console.WriteLine($"\n{Title}");
        foreach (var option in Options)
        {
            Console.WriteLine($" - {option.Key}: {option.Value} votes");
        }
    }
}
