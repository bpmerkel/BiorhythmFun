namespace BiorthymFun.Client.Svg;

/// <summary>
/// Base class for SVG elements that can have a stroke.
/// </summary>
public class strokeBase
{
    /// <summary>
    /// Gets or sets the stroke color of the SVG element.
    /// </summary>
    public string stroke { get; set; }

    /// <summary>
    /// Gets or sets the stroke width of the SVG element.
    /// </summary>
    public double stroke_width { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the stroke line cap of the SVG element.
    /// </summary>
    public StrokeLinecap stroke_linecap { get; set; } = StrokeLinecap.none;

    /// <summary>
    /// Gets or sets the stroke dash array of the SVG element.
    /// </summary>
    public string stroke_dasharray { get; set; }

    /// <summary>
    /// Gets or sets the stroke dash offset of the SVG element.
    /// </summary>
    public double stroke_dashoffset { get; set; }

    /// <summary>
    /// Gets or sets the stroke line join of the SVG element.
    /// </summary>
    public StrokeLineJoin stroke_linejoin { get; set; } = StrokeLineJoin.none;
}
