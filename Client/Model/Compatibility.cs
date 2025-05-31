namespace BiorhythmFun.Client.Model;

/// <summary>
/// Represents a compatibility between two chartable items.
/// </summary>
public class Compatibility : ChartableBase
{
    /// <summary>
    /// Gets or sets the ID of the first chartable item.
    /// </summary>
    public string ID1 { get; set; }

    /// <summary>
    /// Gets or sets the ID of the second chartable item.
    /// </summary>
    public string ID2 { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Compatibility"/> class.
    /// </summary>
    /// <param name="name">The name of the compatibility.</param>
    /// <param name="id1">The ID of the first chartable item.</param>
    /// <param name="id2">The ID of the second chartable item.</param>
    public Compatibility(string name, string id1, string id2)
    {
        Name = name;
        ID1 = id1;
        ID2 = id2;
    }
}