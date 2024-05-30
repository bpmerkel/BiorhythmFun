namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a radial gradient element in SVG.
/// </summary>
public class radialGradient : IBaseElement
{
    /// <summary>
    /// Gets or sets the ID of the radial gradient element.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the x-coordinate of the center of the radial gradient.
    /// </summary>
    public string cx { get; set; }

    /// <summary>
    /// Gets or sets the y-coordinate of the center of the radial gradient.
    /// </summary>
    public string cy { get; set; }

    /// <summary>
    /// Gets or sets the radius of the radial gradient.
    /// </summary>
    public string r { get; set; }

    /// <summary>
    /// Gets or sets the x-coordinate of the focal point of the radial gradient.
    /// </summary>
    public string fx { get; set; }

    /// <summary>
    /// Gets or sets the y-coordinate of the focal point of the radial gradient.
    /// </summary>
    public string fy { get; set; }

    /// <summary>
    /// Gets or sets the radius of the focal circle of the radial gradient.
    /// </summary>
    public string fr { get; set; }

    /// <summary>
    /// Gets or sets the children of the radial gradient element.
    /// </summary>
    public ICollection<IBaseElement> Children { get; set; } = [];
}
