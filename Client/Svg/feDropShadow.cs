namespace BiorthymFun.Client.Svg;

public class feDropShadow : IBaseElement
{
    public string id { get; set; }
    public double dx { get; set; } = double.NaN;
    public double dy { get; set; } = double.NaN;
    public double stdDeviation { get; set; } = double.NaN;
    public string flood_color { get; set; }
    public double flood_opacity { get; set; } = double.NaN;
}
