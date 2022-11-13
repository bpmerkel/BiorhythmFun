namespace BiorthymFun.Client.Svg;

public class path : strokeBase, IBaseElement, IEventBase
{
    public string? id { get; set; } = null;
    public string? d { get; set; } = null;
    public string? fill { get; set; } = null;
    public string? filter { get; set; } = null;
    public string? transform { get; set; } = null;
    public double opacity { get; set; } = double.NaN;
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
