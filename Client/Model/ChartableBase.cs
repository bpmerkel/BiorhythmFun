namespace BiorhythmFun.Client.Model;

/// <summary>
/// Represents the base class for chartable items.
/// </summary>
public class ChartableBase
{
    /// <summary>
    /// Gets the unique identifier for the chartable item.
    /// </summary>
    public string ID { get; init; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the chartable item.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
