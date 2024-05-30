namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a line in SVG.
/// </summary>
public class line : strokeBase, IBaseElement, IEventBase
{
    /// <summary>
    /// Gets or sets the ID of the line.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the x-coordinate of the start of the line.
    /// </summary>
    public double x1 { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the y-coordinate of the start of the line.
    /// </summary>
    public double y1 { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the x-coordinate of the end of the line.
    /// </summary>
    public double x2 { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the y-coordinate of the end of the line.
    /// </summary>
    public double y2 { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the style of the line.
    /// </summary>
    public string style { get; set; }

    /// <summary>
    /// Gets or sets the filter effect for the line.
    /// </summary>
    public string filter { get; set; }

    /// <summary>
    /// Gets or sets the transformation applied to the line.
    /// </summary>
    public string transform { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the line.
    /// </summary>
    public double opacity { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the children of the line.
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
