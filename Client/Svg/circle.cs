namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a circle in SVG.
/// </summary>
public class circle : strokeBase, IBaseElement, IEventBase
{
    /// <summary>
    /// Gets or sets the ID of the circle.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the x-coordinate of the center of the circle.
    /// </summary>
    public double cx { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the y-coordinate of the center of the circle.
    /// </summary>
    public double cy { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the radius of the circle.
    /// </summary>
    public double r { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the fill color of the circle.
    /// </summary>
    public string fill { get; set; }

    /// <summary>
    /// Gets or sets the filter effect for the circle.
    /// </summary>
    public string filter { get; set; }

    /// <summary>
    /// Gets or sets the transformation applied to the circle.
    /// </summary>
    public string transform { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the circle.
    /// </summary>
    public double opacity { get; set; } = 1d;

    /// <summary>
    /// Gets or sets the children of the circle.
    /// </summary>
    public ICollection<IBaseElement> Children { get; set; } = [];

    /// <summary>
    /// Gets or sets the onclick event handler.
    /// </summary>
    public string onclick { get; set; }

    /// <summary>
    /// Gets or sets the stop propagation option.
    /// </summary>
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
