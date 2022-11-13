namespace BiorthymFun.Client.Svg;

public class text
{
    public string? id { get; set; } = null;
    public double x { get; set; } = double.NaN;
    public double y { get; set; } = double.NaN;
    public string? fill { get; set; } = null;
    public string? filter { get; set; } = null;
    public string? font_family { get; set; } = null;
    public double font_size { get; set; } = double.NaN;
    public string? font_weight { get; set; } = null;
    public string? text_anchor { get; set; } = null;
    public string dominant_baseline { get; set; } = "middle";
    public double opacity { get; set; } = double.NaN;
    public string? transform_origin { get; set; } = null;
    public string? transform { get; set; } = null;
    public ICollection<object> Children { get; set; } = new List<object>();
    public string? content { get; set; } = null;
}
