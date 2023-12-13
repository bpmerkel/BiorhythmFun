namespace BiorthymFun.Client.Svg;

public class polyline : IBaseElement, IEventBase
{
    public string id { get; set; }
    public string points { get; set; }
    public string style { get; set; }
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
