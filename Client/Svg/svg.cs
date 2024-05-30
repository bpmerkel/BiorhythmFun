namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents an SVG element.
/// </summary>
public class svg : IBaseElement
{
    /// <summary>
    /// Gets or sets the ID of the SVG element.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the width of the SVG element.
    /// </summary>
    public double width { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the height of the SVG element.
    /// </summary>
    public double height { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the XML namespace of the SVG element.
    /// </summary>
    public string xmlns { get; set; }

    /// <summary>
    /// Gets or sets the style of the SVG element.
    /// </summary>
    public string style { get; set; }

    /// <summary>
    /// Gets or sets the transform attribute of the SVG element.
    /// </summary>
    public string transform { get; set; }

    /// <summary>
    /// Gets or sets the viewBox attribute of the SVG element.
    /// </summary>
    public string viewBox { get; set; }

    /// <summary>
    /// Gets or sets the preserveAspectRatio attribute of the SVG element.
    /// </summary>
    public string preserveAspectRatio { get; set; }

    /// <summary>
    /// Gets or sets the onclick attribute of the SVG element.
    /// </summary>
    public string onclick { get; set; }

    /// <summary>
    /// Gets or sets the StopPropagation attribute of the SVG element.
    /// </summary>
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;

    /// <summary>
    /// Gets or sets the children of the SVG element.
    /// </summary>
    public ICollection<IBaseElement> Children { get; set; } = [];
}
