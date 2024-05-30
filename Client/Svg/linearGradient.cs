namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a linear gradient in SVG.
/// </summary>
public class linearGradient : IBaseElement
{
    /// <summary>
    /// Gets or sets the ID of the linear gradient.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the x-coordinate of the start of the gradient.
    /// </summary>
    public string x1 { get; set; }

    /// <summary>
    /// Gets or sets the x-coordinate of the end of the gradient.
    /// </summary>
    public string x2 { get; set; }

    /// <summary>
    /// Gets or sets the y-coordinate of the start of the gradient.
    /// </summary>
    public string y1 { get; set; }

    /// <summary>
    /// Gets or sets the y-coordinate of the end of the gradient.
    /// </summary>
    public string y2 { get; set; }

    /// <summary>
    /// Gets or sets the children of the linear gradient.
    /// </summary>
    public ICollection<IBaseElement> Children { get; set; } = [];
}
