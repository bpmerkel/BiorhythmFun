using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace BiorthymFun.Client.Svg;

public class SvgHelper
{
    public Action<MouseEventArgs, int> ActionClicked;

    public Dictionary<string, ElementReference> Elementreferences = new();

    // Recursive
    public void Render<T>(T _Item, int k, RenderTreeBuilder builder, int Par_ID = 0)
    {
        if (_Item is null) return;

        builder.OpenRegion(k++);

        builder.OpenElement(k++, _Item.GetType().Name);

        foreach (var pi in _Item.GetType().GetProperties().Where(x => !x.PropertyType.Name.Contains("ICollection")))
        {
            var IsAllowed = true;
            var _value = pi.GetValue(_Item, null);
            if (_value is null) continue;

            if (pi.PropertyType == typeof(double))
            {
                if (double.IsNaN((double)_value))
                {
                    IsAllowed = false;
                }
                else
                {
                    _value = Math.Round((double)_value, 2);
                }
            }

            if (IsAllowed)
            {
                IsAllowed = _value != null && !string.IsNullOrEmpty(_value.ToString());
            }

            if (IsAllowed)
            {
                if (pi.Name == "stroke_linecap" && _value is StrokeLinecap lc)
                {
                    IsAllowed = lc != StrokeLinecap.none;
                }
            }

            if (IsAllowed)
            {
                if (pi.Name == "stroke_linejoin" && _value is StrokeLineJoin lj)
                {
                    IsAllowed = lj != StrokeLineJoin.none;
                }
            }

            if (IsAllowed)
            {
                var _attrName = pi.Name;

                if (_attrName.Equals("onclick") && _value is BoolOptionsEnum boe)
                {
                    if (boe == BoolOptionsEnum.Yes)
                    {
                        builder.AddAttribute(1, _attrName, EventCallback.Factory.Create<MouseEventArgs>(this, e => Clicked(e, Par_ID)));
                    }
                }
                else if (_attrName.ToLower().Equals("stoppropagation") && _value is BoolOptionsEnum boe2)
                {
                    if (boe2 == BoolOptionsEnum.Yes)
                    {
                        builder.AddEventStopPropagationAttribute(2, "onclick", true);
                    }
                }
                else if(_value is not null)
                {
                    if (_attrName.Equals("content"))
                    {
                        builder.AddContent(3, _value.ToString());
                    }
                    else
                    {
                        if (_attrName.Contains('_'))
                        {
                            _attrName = _attrName.Replace("_", "-");
                        }

                        builder.AddAttribute(4, _attrName, _value.ToString());
                    }
                }
            }
        }

        var Children = _Item.GetType().GetProperty("Children");
        if (Children is not null)
        {
            var children = Children.GetValue(_Item) as List<object>;
            if (children is not null && children.Any())
            {
                foreach (object item in children)
                {
                    Render(item, k++, builder, Par_ID); ;
                }
            }
        }

        builder.CloseElement();
        builder.CloseRegion();
    }

    public void Clicked(MouseEventArgs e, int i) => ActionClicked?.Invoke(e, i);
}
