namespace BiorthymFun.Client.Svg;

public class animateTransform : IBaseElement
{
    public string id { get; set; }
    public string attributeName { get; set; }
    public string type { get; set; }
    public double by { get; set; } = double.NaN;
    public double dur { get; set; } = double.NaN;
    public string repeatCount { get; set; }
    public string values { get; set; }
    public string keyTimes { get; set; }
}
