namespace BiorthymFun.Client.Svg;

public class line : strokeBase, IBaseElement, IEventBase
{
    public string id { get; set; }
    public double x1 { get; set; } = double.NaN;
    public double y1 { get; set; } = double.NaN;
    public double x2 { get; set; } = double.NaN;
    public double y2 { get; set; } = double.NaN;
    public string style { get; set; }
    public string filter { get; set; }
    public string transform { get; set; }
    public double opacity { get; set; } = double.NaN;
    public ICollection<IBaseElement> Children { get; set; } = [];
    public string onclick { get; set; }
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
