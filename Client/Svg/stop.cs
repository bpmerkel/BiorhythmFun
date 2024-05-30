namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a stop element in SVG.
/// </summary>
public class stop : IBaseElement
{
    /// <summary>
    /// Gets or sets the ID of the stop element.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the offset of the stop element.
    /// </summary>
    public string offset { get; set; }

    /// <summary>
    /// Gets or sets the stop color of the stop element.
    /// </summary>
    public string stop_color { get; set; }

    /// <summary>
    /// Gets or sets the stop opacity of the stop element.
    /// </summary>
    public string stop_opacity { get; set; }

    /// <summary>
    /// Gets or sets the style of the stop element.
    /// </summary>
    public string style { get; set; }
}
