namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a polygon element in SVG.
/// </summary>
public class polygon : IBaseElement, IEventBase
{
    /// <summary>
    /// Gets or sets the ID of the polygon element.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the points that make up the polygon.
    /// </summary>
    public string points { get; set; }

    /// <summary>
    /// Gets or sets the style of the polygon element.
    /// </summary>
    public string style { get; set; }

    /// <summary>
    /// Gets or sets the onclick attribute of the polygon element.
    /// </summary>
    public string onclick { get; set; }

    /// <summary>
    /// Gets or sets the StopPropagation attribute of the polygon element.
    /// </summary>
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
