namespace BiorthymFun.Client.Svg;

public class rect : strokeBase, IBaseElement, IEventBase
{
    public string id { get; set; }
    public double x { get; set; } = double.NaN;
    public double y { get; set; } = double.NaN;
    public double rx { get; set; } = double.NaN;
    public double ry { get; set; } = double.NaN;
    public double width { get; set; } = double.NaN;
    public double height { get; set; } = double.NaN;
    public string style { get; set; }
    public string fill { get; set; }
    public string filter { get; set; }
    public double fill_opacity { get; set; } = 1d;
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
    public ICollection<IBaseElement> Children { get; set; } = [];
}