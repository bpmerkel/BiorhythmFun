namespace BiorthymFun.Client.Svg;

public class image : IBaseElement, IEventBase
{
    public string? id { get; set; } = null;
    public double x { get; set; } = double.NaN;
    public double y { get; set; } = double.NaN;
    public double width { get; set; } = double.NaN;
    public double height { get; set; } = double.NaN;
    public string? href { get; set; } = null;
    public string? filter { get; set; } = null;
    public string? transform { get; set; } = null;
    public double opacity { get; set; } = double.NaN;
    public ICollection<object> Children { get; set; } = new List<object>();
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
