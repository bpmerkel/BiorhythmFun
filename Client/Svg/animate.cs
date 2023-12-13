namespace BiorthymFun.Client.Svg;

public class animate: IBaseElement
{
    public string id { get; set; }
    public string attributeName { get; set; }
    public double from { get; set; } = double.NaN;
    public double to { get; set; } = double.NaN;
    public string dur { get; set; } = "1s";
    public string repeatCount { get; set; }
    public string fill { get; set; } = "freeze";
    public string values { get; set; }
    public string keyTimes { get; set; }
    public string begin { get; set; } = "0s";
    public string end { get; set; }
    public string additive { get; set; }    // = "sum";
}