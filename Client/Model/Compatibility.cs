namespace BiorhythmFun.Client.Model;

public class Compatibility : ChartableBase
{
    public string ID1 { get; set; }
    public string ID2 { get; set; }

    public Compatibility(string name, string id1, string id2)
    {
        Name = name;
        ID1 = id1;
        ID2 = id2;
    }
}
