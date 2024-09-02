namespace TrustedVoteLibrary.BallotModel;

public class MultiVoteItem : BallotItem
{
    public int MaxSelections { get; set; }

    public MultiVoteItem(string title, List<string> options, int maxSelections)
        : base(title, options)
    {
        MaxSelections = maxSelections;
    }

    public override void RecordVote(List<string> selections)
    {
        if (selections.Count > MaxSelections)
        {
            Console.WriteLine($"You can only vote for up to {MaxSelections} options.");
            return;
        }

        foreach (var choice in selections)
        {
            if (Options.ContainsKey(choice))
            {
                Options[choice]++;
            }
            else
            {
                Console.WriteLine($"Invalid choice: {choice}");
            }
        }
    }
}
