namespace BiorhythmFun.Client.Model;

/// <summary>
/// Represents a group of chartable items.
/// </summary>
public class Group : ChartableBase
{
    /// <summary>
    /// Gets or sets the IDs of the chartable items in the group.
    /// </summary>
    public List<string> IDs { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Group"/> class.
    /// </summary>
    /// <param name="name">The name of the group.</param>
    /// <param name="ids">The IDs of the chartable items in the group.</param>
    public Group(string name, List<string> ids)
    {
        Name = name;
        IDs = ids;
    }
}
