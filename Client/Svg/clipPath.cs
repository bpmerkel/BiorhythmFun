namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a clipPath element in SVG.
/// </summary>
public class clipPath : IBaseElement
{
    /// <summary>
    /// Gets or sets the ID of the clipPath.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the children of the clipPath.
    /// </summary>
    public ICollection<IBaseElement> Children { get; set; } = [];
}
