namespace BiorthymFun.Client.Svg;

public class line : strokeBase, IBaseElement, IEventBase
{
    public string? id { get; set; } = null;
    public double x1 { get; set; } = double.NaN;
    public double y1 { get; set; } = double.NaN;
    public double x2 { get; set; } = double.NaN;
    public double y2 { get; set; } = double.NaN;
    public string? style { get; set; } = null;
    public string? filter { get; set; } = null;
    public string? transform { get; set; } = null;
    public double opacity { get; set; } = double.NaN;
    public ICollection<object> Children { get; set; } = new List<object>();
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
