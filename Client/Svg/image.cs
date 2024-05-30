namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents an image in SVG.
/// </summary>
public class image : IBaseElement, IEventBase
{
    /// <summary>
    /// Gets or sets the ID of the image.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the x-coordinate of the image.
    /// </summary>
    public double x { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the y-coordinate of the image.
    /// </summary>
    public double y { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the width of the image.
    /// </summary>
    public double width { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the height of the image.
    /// </summary>
    public double height { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the href of the image.
    /// </summary>
    public string href { get; set; }

    /// <summary>
    /// Gets or sets the filter effect for the image.
    /// </summary>
    public string filter { get; set; }

    /// <summary>
    /// Gets or sets the transformation applied to the image.
    /// </summary>
    public string transform { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the image.
    /// </summary>
    public double opacity { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the children of the image.
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
