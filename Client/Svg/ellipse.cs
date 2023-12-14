namespace BiorthymFun.Client.Svg;

public class ellipse : IBaseElement, IEventBase
{
    public string id { get; set; }
    public double cx { get; set; } = double.NaN;
    public double cy { get; set; } = double.NaN;
    public double rx { get; set; } = double.NaN;
    public double ry { get; set; } = double.NaN;
    public string style { get; set; }
    public string fill { get; set; }
    public string filter { get; set; }
    public string onclick { get; set; }
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
