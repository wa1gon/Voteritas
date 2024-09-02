namespace TrustedVoteLibrary.BallotModel;

public class Ballot
{
    public string Title { get; set; }
    public List<BallotItem> Items { get; private set; }

    public Ballot(string title)
    {
        Title = title;
        Items = new List<BallotItem>();
    }

    public void AddItem(BallotItem item)
    {
        Items.Add(item);
    }

    public void DisplayBallot()
    {
        Console.WriteLine(Title);
        foreach (var item in Items)
        {
            item.DisplayItem();
        }
    }

    public void Vote(string itemTitle, List<string> selections)
    {
        var item = Items.Find(i => i.Title.Equals(itemTitle, StringComparison.OrdinalIgnoreCase));
        if (item != null)
        {
            item.RecordVote(selections);
        }
        else
        {
            Console.WriteLine("Item not found.");
        }
    }
}
