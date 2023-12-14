namespace BiorthymFun.Client.Svg;

public class svg : IBaseElement
{
    public string id { get; set; }
    public double width { get; set; } = double.NaN;
    public double height { get; set; } = double.NaN;
    public string xmlns { get; set; }
    public string style { get; set; }
    public string transform { get; set; }
    public string viewBox { get; set; }
    public string preserveAspectRatio { get; set; }
    public string onclick { get; set; }
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
    public ICollection<IBaseElement> Children { get; set; } = [];
}