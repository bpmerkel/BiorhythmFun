namespace BiorthymFun.Client.Svg;

/// <summary>
/// Represents a filter in SVG.
/// </summary>
public class filter : IBaseElement
{
    /// <summary>
    /// Gets or sets the ID of the filter.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets or sets the units to define filter effect region.
    /// </summary>
    public string filterUnits { get; set; }

    /// <summary>
    /// Gets or sets the units to define primitive filter subregion.
    /// </summary>
    public string primitiveUnits { get; set; }

    /// <summary>
    /// Gets or sets the x-axis co-ordinate.
    /// </summary>
    public string x { get; set; }

    /// <summary>
    /// Gets or sets the y-axis co-ordinate.
    /// </summary>
    public string y { get; set; }

    /// <summary>
    /// Gets or sets the length.
    /// </summary>
    public string width { get; set; }

    /// <summary>
    /// Gets or sets the length.
    /// </summary>
    public string height { get; set; }

    /// <summary>
    /// Gets or sets the numbers for filter region.
    /// </summary>
    public string filterRes { get; set; }

    /// <summary>
    /// Gets or sets the children of the filter.
    /// </summary>
    public ICollection<IBaseElement> Children { get; set; } = [];
}
