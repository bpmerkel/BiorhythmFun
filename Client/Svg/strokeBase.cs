namespace BiorthymFun.Client.Svg;

public class strokeBase
{
    public string stroke { get; set; } = null;
    public double stroke_width { get; set; } = double.NaN;
    public StrokeLinecap stroke_linecap { get; set; } = StrokeLinecap.none;
    public string stroke_dasharray { get; set; } = null;
    public StrokeLineJoin stroke_linejoin { get; set; } = StrokeLineJoin.none;
}
