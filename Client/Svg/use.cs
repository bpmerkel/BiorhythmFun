namespace BiorthymFun.Client.Svg;

public class use : IBaseElement
{
    public string href { get; set; }
    public string id { get; set; }
    public double x { get; set; } = double.NaN;
    public double y { get; set; } = double.NaN;
    public string content { get; set; }
    public ICollection<IBaseElement> Children { get; set; } = [];
}