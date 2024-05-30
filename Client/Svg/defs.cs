namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a defs element in SVG.
/// </summary>
public class defs : IBaseElement
{
    /// <summary>
    /// Gets or sets the ID of the defs.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the children of the defs.
    /// </summary>
    public ICollection<IBaseElement> Children { get; set; } = [];
}
