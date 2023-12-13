namespace BiorthymFun.Client.Svg;

public class linearGradient : IBaseElement
{
    public string id { get; set; }
    public string x1 { get; set; }
    public string x2 { get; set; }
    public string y1 { get; set; }
    public string y2 { get; set; }
    public ICollection<IBaseElement> Children { get; set; } = [];
}