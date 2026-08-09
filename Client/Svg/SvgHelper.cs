namespace BiorhythmFun.Client.Svg;

public class SvgHelper
{
    public Action<MouseEventArgs, int> ActionClicked;

    // Recursive
    public void Render<T>(T Item, int K, RenderTreeBuilder Builder, int Parent = 0)
    {
        if (Item is null) return;

        Builder.OpenRegion(K++);
        Builder.OpenElement(K++, Item.GetType().Name);

        var properties = Item.GetType().GetProperties()
            .Where(x => !x.PropertyType.Name.Contains("ICollection"));

        foreach (var pi in properties)
        {
            var value = pi.GetValue(Item, null);
            if (value is null) continue;
            var isAllowed = true;

            if (pi.PropertyType == typeof(double))
            {
                if (double.IsNaN((double)value))
                {
                    isAllowed = false;
                }
                else
                {
                    value = Math.Round((double)value, 2);
                }
            }

            if (isAllowed)
            {
                isAllowed = value != null && !string.IsNullOrEmpty(value.ToString());
            }

            if (isAllowed)
            {
                if (pi.Name == "stroke_linecap" && value is StrokeLinecap lc)
                {
                    isAllowed = lc != StrokeLinecap.none;
                }
            }

            if (isAllowed)
            {
                if (pi.Name == "stroke_linejoin" && value is StrokeLineJoin lj)
                {
                    isAllowed = lj != StrokeLineJoin.none;
                }
            }

            if (isAllowed)
            {
                var attrName = pi.Name;

                if (attrName.Equals("onclick") && value is BoolOptionsEnum boe)
                {
                    if (boe == BoolOptionsEnum.Yes)
                    {
                        Builder.AddAttribute(1, attrName, EventCallback.Factory.Create<MouseEventArgs>(this, e => Clicked(e, Parent)));
                    }
                }
                else if (attrName.ToLower().Equals("stoppropagation") && value is BoolOptionsEnum boe2)
                {
                    if (boe2 == BoolOptionsEnum.Yes)
                    {
                        Builder.AddEventStopPropagationAttribute(2, "onclick", true);
                    }
                }
                else if (value is not null)
                {
                    if (attrName.Equals("content"))
                    {
                        Builder.AddContent(3, value.ToString());
                    }
                    else
                    {
                        if (attrName.Contains('_'))
                        {
                            attrName = attrName.Replace("_", "-");
                        }

                        Builder.AddAttribute(4, attrName, value.ToString());
                    }
                }
            }
        }

        var Children = Item.GetType().GetProperty("Children");
        if (Children is not null)
        {
            var children = Children.GetValue(Item) as ICollection<IBaseElement>;
            if (children is not null && children.Count != 0)
            {
                foreach (var item in children)
                {
                    Render(item, K++, Builder, Parent);
                }
            }
        }

        Builder.CloseElement();
        Builder.CloseRegion();
    }

    public void Clicked(MouseEventArgs e, int i) => ActionClicked?.Invoke(e, i);
}