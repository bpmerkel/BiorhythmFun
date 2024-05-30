namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a rectangle element in SVG.
/// </summary>
public class rect : strokeBase, IBaseElement, IEventBase
{
    /// <summary>
    /// Gets or sets the ID of the rectangle element.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the x-coordinate of the rectangle element.
    /// </summary>
    public double x { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the y-coordinate of the rectangle element.
    /// </summary>
    public double y { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the x-radius of the rectangle element.
    /// </summary>
    public double rx { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the y-radius of the rectangle element.
    /// </summary>
    public double ry { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the width of the rectangle element.
    /// </summary>
    public double width { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the height of the rectangle element.
    /// </summary>
    public double height { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the style of the rectangle element.
    /// </summary>
    public string style { get; set; }

    /// <summary>
    /// Gets or sets the fill color of the rectangle element.
    /// </summary>
    public string fill { get; set; }

    /// <summary>
    /// Gets or sets the filter effect for the rectangle element.
    /// </summary>
    public string filter { get; set; }

    /// <summary>
    /// Gets or sets the fill opacity of the rectangle element.
    /// </summary>
    public double fill_opacity { get; set; } = 1d;

    /// <summary>
    /// Gets or sets the onclick attribute of the rectangle element.
    /// </summary>
    public string onclick { get; set; }

    /// <summary>
    /// Gets or sets the onmouseover attribute of the rectangle element.
    /// </summary>
    public string onmouseover { get; set; }

    /// <summary>
    /// Gets or sets the StopPropagation attribute of the rectangle element.
    /// </summary>
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;

    /// <summary>
    /// Gets or sets the children of the rectangle element.
    /// </summary>
    public ICollection<IBaseElement> Children { get; set; } = [];
}
