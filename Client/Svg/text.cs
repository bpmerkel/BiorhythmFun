namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a text element in SVG.
/// </summary>
public class text : IBaseElement
{
    /// <summary>
    /// Gets or sets the ID of the text element.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the x-coordinate of the text element.
    /// </summary>
    public double x { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the y-coordinate of the text element.
    /// </summary>
    public double y { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the fill color of the text element.
    /// </summary>
    public string fill { get; set; }

    /// <summary>
    /// Gets or sets the filter effect for the text element.
    /// </summary>
    public string filter { get; set; }

    /// <summary>
    /// Gets or sets the font family of the text element.
    /// </summary>
    public string font_family { get; set; }

    /// <summary>
    /// Gets or sets the font size of the text element.
    /// </summary>
    public double font_size { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the font weight of the text element.
    /// </summary>
    public string font_weight { get; set; }

    /// <summary>
    /// Gets or sets the text anchor of the text element.
    /// </summary>
    public string text_anchor { get; set; }

    /// <summary>
    /// Gets or sets the dominant baseline of the text element.
    /// </summary>
    public string dominant_baseline { get; set; } = "middle";

    /// <summary>
    /// Gets or sets the opacity of the text element.
    /// </summary>
    public double opacity { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the transform origin of the text element.
    /// </summary>
    public string transform_origin { get; set; }

    /// <summary>
    /// Gets or sets the transform of the text element.
    /// </summary>
    public string transform { get; set; }

    /// <summary>
    /// Gets or sets the content of the text element.
    /// </summary>
    public string content { get; set; }

    /// <summary>
    /// Gets or sets the children of the text element.
    /// </summary>
    public ICollection<IBaseElement> Children { get; set; } = [];
}
