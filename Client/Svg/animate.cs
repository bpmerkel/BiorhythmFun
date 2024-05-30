namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents an animate element in SVG.
/// </summary>
public class animate : IBaseElement
{
    /// <summary>
    /// Gets or sets the ID of the animate.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the name of the attribute to animate.
    /// </summary>
    public string attributeName { get; set; }

    /// <summary>
    /// Gets or sets the starting value of the animation.
    /// </summary>
    public double from { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the ending value of the animation.
    /// </summary>
    public double to { get; set; } = double.NaN;

    /// <summary>
    /// Gets or sets the duration of the animation.
    /// </summary>
    public string dur { get; set; } = "1s";

    /// <summary>
    /// Gets or sets the repeat count of the animation.
    /// </summary>
    public string repeatCount { get; set; }

    /// <summary>
    /// Gets or sets the fill attribute of the animation.
    /// </summary>
    public string fill { get; set; } = "freeze";

    /// <summary>
    /// Gets or sets the values of the animation.
    /// </summary>
    public string values { get; set; }

    /// <summary>
    /// Gets or sets the key times of the animation.
    /// </summary>
    public string keyTimes { get; set; }

    /// <summary>
    /// Gets or sets the begin time of the animation.
    /// </summary>
    public string begin { get; set; } = "0s";

    /// <summary>
    /// Gets or sets the end time of the animation.
    /// </summary>
    public string end { get; set; }

    /// <summary>
    /// Gets or sets the additive attribute of the animation.
    /// </summary>
    public string additive { get; set; }    // = "sum";
}
