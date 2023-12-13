namespace BiorthymFun.Client.Svg;

public class path : strokeBase, IBaseElement, IEventBase
{
    public string id { get; set; }
    public string d { get; set; }
    public string fill { get; set; }
    public string filter { get; set; }
    public string transform { get; set; }
    public double opacity { get; set; } = double.NaN;
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
    public ICollection<IBaseElement> Children { get; set; } = [];
}