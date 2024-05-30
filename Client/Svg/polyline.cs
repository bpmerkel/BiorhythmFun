namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a polyline element in SVG.
/// </summary>
public class polyline : IBaseElement, IEventBase
{
    /// <summary>
    /// Gets or sets the ID of the polyline element.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the points that make up the polyline.
    /// </summary>
    public string points { get; set; }

    /// <summary>
    /// Gets or sets the style of the polyline element.
    /// </summary>
    public string style { get; set; }

    /// <summary>
    /// Gets or sets the onclick attribute of the polyline element.
    /// </summary>
    public string onclick { get; set; }

    /// <summary>
    /// Gets or sets the StopPropagation attribute of the polyline element.
    /// </summary>
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
