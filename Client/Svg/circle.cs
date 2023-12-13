namespace BiorthymFun.Client.Svg;

public class circle : strokeBase, IBaseElement, IEventBase
{
    public string id { get; set; }
    public double cx { get; set; } = double.NaN;
    public double cy { get; set; } = double.NaN;
    public double r { get; set; } = double.NaN;
    public string fill { get; set; }
    public string filter { get; set; }
    public string transform { get; set; }
    public double opacity { get; set; } = 1d;
    public ICollection<IBaseElement> Children { get; set; } = [];
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
