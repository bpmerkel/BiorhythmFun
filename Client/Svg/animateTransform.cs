namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents an animateTransform element in SVG.
/// </summary>
public class animateTransform : IBaseElement
{
    /// <summary>
    /// Gets or sets the ID of the animateTransform.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the name of the attribute to animate.
    /// </summary>
    public string attributeName { get; set; }

    /// <summary>
    /// Gets or sets the type of the animation.
    /// </summary>
    public string type { get; set; }

    /// <summary>
    /// Gets or sets the by value of the animation.
    /// </summary>
    public double by { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the duration of the animation.
    /// </summary>
    public double dur { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the repeat count of the animation.
    /// </summary>
    public string repeatCount { get; set; }

    /// <summary>
    /// Gets or sets the values of the animation.
    /// </summary>
    public string values { get; set; }

    /// <summary>
    /// Gets or sets the key times of the animation.
    /// </summary>
    public string keyTimes { get; set; }
}
