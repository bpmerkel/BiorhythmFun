namespace BiorthymFun.Client.Svg;

public class filter : IBaseElement
{
    public string id { get; set; }
    // "units to define filter effect region"
    public string filterUnits { get; set; }
    // "units to define primitive filter subregion"
    public string primitiveUnits { get; set; }
    // "x-axis co-ordinate"
    public string x { get; set; }
    // "y-axis co-ordinate"
    public string y { get; set; }
    // "length"
    public string width { get; set; }
    // "length"
    public string height { get; set; }
    // "numbers for filter region"
    public string filterRes { get; set; }
    public ICollection<IBaseElement> Children { get; set; } = [];
}