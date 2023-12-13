namespace BiorthymFun.Client.Svg;

public class text : IBaseElement
{
    public string id { get; set; }
    public double x { get; set; } = double.NaN;
    public double y { get; set; } = double.NaN;
    public string fill { get; set; }
    public string filter { get; set; }
    public string font_family { get; set; }
    public double font_size { get; set; } = double.NaN;
    public string font_weight { get; set; }
    public string text_anchor { get; set; }
    public string dominant_baseline { get; set; } = "middle";
    public double opacity { get; set; } = double.NaN;
    public string transform_origin { get; set; }
    public string transform { get; set; }
    public string content { get; set; }
    public ICollection<IBaseElement> Children { get; set; } = [];
}