namespace BiorthymFun.Client.Svg;

public class ellipse : IBaseElement, IEventBase
{
    public string? id { get; set; } = null;
    public double cx { get; set; } = double.NaN;
    public double cy { get; set; } = double.NaN;
    public double rx { get; set; } = double.NaN;
    public double ry { get; set; } = double.NaN;
    public string? style { get; set; } = null;
    public string? fill { get; set; } = null;
    public string? filter { get; set; } = null;
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
