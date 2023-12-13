namespace BiorthymFun.Client.Svg;

public class clipPath : IBaseElement
{
    public string id { get; set; }
    public ICollection<IBaseElement> Children { get; set; } = [];
}