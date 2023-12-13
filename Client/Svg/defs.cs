namespace BiorthymFun.Client.Svg;

public class defs : IBaseElement
{
    public string id { get; set; }
    public ICollection<IBaseElement> Children { get; set; } = [];
}