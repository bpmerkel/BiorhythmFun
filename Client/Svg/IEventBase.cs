namespace BiorthymFun.Client.Svg;

public interface IEventBase
{
    public string onclick { get; set; }

    public BoolOptionsEnum StopPropagation { get; set; }
}
