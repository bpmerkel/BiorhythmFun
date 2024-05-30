namespace BiorthymFun.Client.Svg;

/// <summary>
/// Interface for SVG elements that can handle events.
/// </summary>
public interface IEventBase
{
    /// <summary>
    /// Gets or sets the onclick event handler.
    /// </summary>
    public string onclick { get; set; }

    /// <summary>
    /// Gets or sets the stop propagation option.
    /// </summary>
    public BoolOptionsEnum StopPropagation { get; set; }
}
