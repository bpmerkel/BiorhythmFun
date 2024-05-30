namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents an ellipse in SVG.
/// </summary>
public class ellipse : IBaseElement, IEventBase
{
    /// <summary>
    /// Gets or sets the ID of the ellipse.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the x-coordinate of the center of the ellipse.
    /// </summary>
    public double cx { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the y-coordinate of the center of the ellipse.
    /// </summary>
    public double cy { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the x-radius of the ellipse.
    /// </summary>
    public double rx { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the y-radius of the ellipse.
    /// </summary>
    public double ry { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the style of the ellipse.
    /// </summary>
    public string style { get; set; }

    /// <summary>
    /// Gets or sets the fill color of the ellipse.
    /// </summary>
    public string fill { get; set; }

    /// <summary>
    /// Gets or sets the filter effect for the ellipse.
    /// </summary>
    public string filter { get; set; }

    /// <summary>
    /// Gets or sets the onclick event handler.
    /// </summary>
    public string onclick { get; set; }

    /// <summary>
    /// Gets or sets the stop propagation option.
    /// </summary>
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
