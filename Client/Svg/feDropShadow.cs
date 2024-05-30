namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a drop shadow filter in SVG.
/// </summary>
public class feDropShadow : IBaseElement
{
    /// <summary>
    /// Gets or sets the ID of the drop shadow filter.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the x-axis offset of the drop shadow.
    /// </summary>
    public double dx { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the y-axis offset of the drop shadow.
    /// </summary>
    public double dy { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the standard deviation of the drop shadow.
    /// </summary>
    public double stdDeviation { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the color of the drop shadow.
    /// </summary>
    public string flood_color { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the drop shadow.
    /// </summary>
    public double flood_opacity { get; set; } = double.NaN;
}
