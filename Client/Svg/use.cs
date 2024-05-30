namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a use element in SVG.
/// </summary>
public class use : IBaseElement
{
    /// <summary>
    /// Gets or sets the href attribute of the use element.
    /// </summary>
    public string href { get; set; }

    /// <summary>
    /// Gets or sets the id attribute of the use element.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the x-coordinate of the use element.
    /// </summary>
    public double x { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the y-coordinate of the use element.
    /// </summary>
    public double y { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the content of the use element.
    /// </summary>
    public string content { get; set; }

    /// <summary>
    /// Gets or sets the children of the use element.
    /// </summary>
    public ICollection<IBaseElement> Children { get; set; } = [];
}
