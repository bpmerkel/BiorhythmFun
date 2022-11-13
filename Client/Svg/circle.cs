using System.Collections.Generic;

namespace BiorthymFun.Client.Svg;

public class circle : strokeBase, IBaseElement, IEventBase
{
    public string? id { get; set; } = null;
    public bool CaptureRef { get; set; } = false;
    public double cx { get; set; } = double.NaN;
    public double cy { get; set; } = double.NaN;
    public double r { get; set; } = double.NaN;
    public string? fill { get; set; } = null;
    public string? filter { get; set; } = null;
    public string? transform { get; set; } = null;
    public ICollection<object> Children { get; set; } = new List<object>();
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
