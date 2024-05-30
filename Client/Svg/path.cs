namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a path element in SVG.
/// </summary>
public class path : strokeBase, IBaseElement, IEventBase
{
    /// <summary>
    /// Gets or sets the ID of the path.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the path data.
    /// </summary>
    public string d { get; set; }

    /// <summary>
    /// Gets or sets the fill color of the path.
    /// </summary>
    public string fill { get; set; }

    /// <summary>
    /// Gets or sets the filter effect for the path.
    /// </summary>
    public string filter { get; set; }

    /// <summary>
    /// Gets or sets the transformation applied to the path.
    /// </summary>
    public string transform { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the path.
    /// </summary>
    public double opacity { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the onclick event handler.
    /// </summary>
    public string onclick { get; set; }

    /// <summary>
    /// Gets or sets the onmouseover event handler.
    /// </summary>
    public string onmouseover { get; set; }

    /// <summary>
    /// Gets or sets the stop propagation option.
    /// </summary>
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;

    /// <summary>
    /// Gets or sets the children of the path.
    /// </summary>
    public ICollection<IBaseElement> Children { get; set; } = [];
}
