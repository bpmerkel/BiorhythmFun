namespace BiorhythmFun.Client.Model;

public class ChartableBase
{
    public string ID { get; init; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
}
