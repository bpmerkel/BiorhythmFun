namespace BiorthymFun.Client.Svg;

public class image : IBaseElement, IEventBase
{
    public string id { get; set; }
    public double x { get; set; } = double.NaN;
    public double y { get; set; } = double.NaN;
    public double width { get; set; } = double.NaN;
    public double height { get; set; } = double.NaN;
    public string href { get; set; }
    public string filter { get; set; }
    public string transform { get; set; }
    public double opacity { get; set; } = double.NaN;
    public ICollection<IBaseElement> Children { get; set; } = [];
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
