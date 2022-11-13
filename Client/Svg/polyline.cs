namespace BiorthymFun.Client.Svg;

public class polyline : IBaseElement, IEventBase
{
    public string? id { get; set; } = null;
    public bool CaptureRef { get; set; } = false;
    public string? points { get; set; } = null;
    public string? style { get; set; } = null;
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
