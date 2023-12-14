namespace BiorthymFun.Client.Svg;

public class polygon : IBaseElement, IEventBase
{
    public string id { get; set; }
    public string points { get; set; }
    public string style { get; set; }
    public string onclick { get; set; }
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
