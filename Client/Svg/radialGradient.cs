namespace BiorthymFun.Client.Svg;

public class radialGradient : IBaseElement
{
    public string id { get; set; }
    public string cx { get; set; }
    public string cy { get; set; }
    public string r { get; set; }
    public string fx { get; set; }
    public string fy { get; set; }
    public string fr { get; set; }
    public ICollection<IBaseElement> Children { get; set; } = [];
}