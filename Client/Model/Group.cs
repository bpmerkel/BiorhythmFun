namespace BiorhythmFun.Client.Model;

public class Group : ChartableBase
{
    public List<string> IDs { get; set; }

    public Group(string name, List<string> ids)
    {
        Name = name;
        IDs = ids;
    }
}
