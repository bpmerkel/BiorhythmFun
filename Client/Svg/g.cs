namespace BiorthymFun.Client.Svg;

public class g : strokeBase, IBaseElement, IEventBase
{
    public string id { get; set; }
    public double font_size { get; set; } = double.NaN;
    public string font_family { get; set; }
    public string text_anchor { get; set; }
    public string fill { get; set; }
    public string clip_path { get; set; }
    public string transform { get; set; }
    public ICollection<IBaseElement> Children { get; set; } = [];
    public BoolOptionsEnum onclick { get; set; } = BoolOptionsEnum.none;
    public BoolOptionsEnum StopPropagation { get; set; } = BoolOptionsEnum.none;
}
