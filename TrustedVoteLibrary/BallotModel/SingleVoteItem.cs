namespace TrustedVoteLibrary.BallotModel;

public class SingleVoteItem : BallotItem
{
    public SingleVoteItem(string title, List<string> options)
        : base(title, options)
    {
    }

    public override void RecordVote(List<string> selections)
    {
        if (selections.Count != 1)
        {
            Console.WriteLine("You can only vote for one option.");
            return;
        }

        string choice = selections[0];
        if (Options.ContainsKey(choice))
        {
            Options[choice]++;
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
}
