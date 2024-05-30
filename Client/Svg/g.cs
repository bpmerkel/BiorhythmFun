namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a group of elements in SVG.
/// </summary>
public class g : strokeBase, IBaseElement, IEventBase
{
    /// <summary>
    /// Gets or sets the ID of the group.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the font size for the text in the group.
    /// </summary>
    public double font_size { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the font family for the text in the group.
    /// </summary>
    public string font_family { get; set; }

    /// <summary>
    /// Gets or sets the text anchor for the text in the group.
    /// </summary>
    public string text_anchor { get; set; }

    /// <summary>
    /// Gets or sets the fill color for the group.
    /// </summary>
    public string fill { get; set; }

    /// <summary>
    /// Gets or sets the clip path for the group.
    /// </summary>
    public string clip_path { get; set; }

    /// <summary>
    /// Gets or sets the transformation applied to the group.
    /// </summary>
    public string transform { get; set; }

    /// <summary>
    /// Gets or sets the children of the group.
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
