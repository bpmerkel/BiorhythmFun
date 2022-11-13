namespace BiorthymFun.Client.Svg;

public interface IEventBase
{
    public BoolOptionsEnum onclick { get; set; }

    public BoolOptionsEnum StopPropagation { get; set; }
}
