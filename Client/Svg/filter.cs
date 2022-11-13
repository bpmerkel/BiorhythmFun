namespace BiorthymFun.Client.Svg;

public class filter
{
    public string? id { get; set; } = null;
    // "units to define filter effect region"
    public string? filterUnits { get; set; } = null;
    // "units to define primitive filter subregion"
    public string? primitiveUnits { get; set; } = null;
    // "x-axis co-ordinate"
    public string? x { get; set; } = null;
    // "y-axis co-ordinate"
    public string? y { get; set; } = null;
    // "length"
    public string? width { get; set; } = null;
    // "length"
    public string? height { get; set; } = null;
    // "numbers for filter region"
    public string? filterRes { get; set; } = null;
    public ICollection<object> Children { get; set; } = new List<object>();
}
