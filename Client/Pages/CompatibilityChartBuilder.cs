// <copyright file="CompatibilityChartBuilder.cs" company="Brady P. Merkel">
// Copyright (c) Brady P. Merkel. All rights reserved.
// </copyright>

namespace BiorhythmFun.Client.Pages;

/// <summary>
/// CompatibilityChartBuilder component
/// </summary>
public class CompatibilityChartBuilder : ComponentBase
{
    private const string FontFamily = "arial";
    private const string ShadowColor = "#707070";
    private const string RedColor = "#FF0000";
    private const string DarkRedColor = "#aa0000";
    private const string BlueColor = "#0000FF";
    private const string DarkBlueColor = "#0000aa";
    private static readonly string GreenColor = FromRgb(0, 128, 0);
    private static readonly string DarkGreenColor = FromRgb(0, 96, 0);

    /// <summary>
    ///  Gets or sets the 1st birthdate.
    /// </summary>
    [Parameter]
    public DateTime Birthdate1 { get; set; }

    /// <summary>
    ///  Gets or sets the 2nd birthdate.
    /// </summary>
    [Parameter]
    public DateTime Birthdate2 { get; set; }

    /// <summary>
    /// Gets or sets the Height.
    /// </summary>
    [Parameter]
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the Width.
    /// </summary>
    [Parameter]
    public int Width { get; set; }

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder) => new SvgHelper().Render(Chart(), 0, builder);

    private static string FromRgb(int r, int g, int b) => $"#{r:X2}{g:X2}{b:X2}";

    private svg Chart()
    {
        var svg = new svg
        {
            id = DateTime.Now.ToString("HHmmssffff"),
            height = Height,
            width = Width,
            xmlns = "http://www.w3.org/2000/svg"
        };

        var defs1 = new defs();
        var gradients = new[]
        {
            ( "grad1", "#EEEEEE", "#CCCCCC" ),
            ( "redgradient", RedColor, DarkRedColor ),
            ( "greengradient", GreenColor, DarkGreenColor ),
            ( "bluegradient", BlueColor, DarkBlueColor )
        };
        foreach (var grad in gradients)
        {
            var linear1 = new linearGradient { id = grad.Item1, x1 = "0%", y1 = "0%", x2 = "100%", y2 = "100%" };
            linear1.Children.Add(new stop { stop_color = grad.Item2, offset = "0%" });
            linear1.Children.Add(new stop { stop_color = grad.Item3, offset = "100%" });
            defs1.Children.Add(linear1);
        }

        var filter = new filter { id = "dropShadow" };
        filter.Children.Add(new feDropShadow { dx = .5d, dy = .5d, stdDeviation = .7d, flood_color = ShadowColor, flood_opacity = 1d });
        defs1.Children.Add(filter);

        svg.Children.Add(defs1);

        var daysdiff = Math.Abs((Birthdate1.Date - Birthdate2.Date).TotalDays);
        var p = 1d - daysdiff % 23d / 23d;
        var e = 1d - daysdiff % 28d / 28d;
        var i = 1d - daysdiff % 33d / 33d;

        // draw frame and background
        svg.Children.Add(new rect { x = 0, y = 0, width = Width, height = Height, fill = "url('#grad1')", stroke_width = 3, stroke = "#BBBBBB" });

        // draw grid lines
        var xmin = 100;
        var xmax = Width - 10;
        Enumerable.Range(0, 11)
            .Select(x => x / 10d)
            .ToList()
            .ForEach(f =>
            {
                var x = f * (xmax - xmin) + xmin;
                svg.Children.Add(new line { x1 = x, y1 = 10, x2 = x, y2 = Height - 10, stroke_width = 1, stroke = "#BBBBBB" });
                svg.Children.Add(new text
                {
                    content = $"{f:P0}",
                    x = x - 2,
                    y = Height / 3,
                    fill = "#BBBBBB",
                    text_anchor = "end",
                    dominant_baseline = "middle",
                    font_family = FontFamily,
                    font_size = 8,
                    font_weight = "normal",
                    filter = "url(#dropShadow)"
                });
            });

        // draw bars
        svg.Children.Add(new rect { x = xmin, y = 10, width = p * (xmax - xmin), height = Height / 3 - 20, fill = "url('#redgradient')", stroke_width = 1, stroke = DarkRedColor, filter = "url(#dropShadow)" });
        svg.Children.Add(new rect { x = xmin, y = Height / 3 + 10, width = e * (xmax - xmin), height = Height / 3 - 20, fill = "url('#greengradient')", stroke_width = 1, stroke = DarkGreenColor, filter = "url(#dropShadow)" });
        svg.Children.Add(new rect { x = xmin, y = 2 * Height / 3 + 10, width = i * (xmax - xmin), height = Height / 3 - 20, fill = "url('#bluegradient')", stroke_width = 1, stroke = DarkBlueColor, filter = "url(#dropShadow)" });

        // draw text labels Physical, Emotional, Intellectual
        svg.Children.Add(new text
        {
            content = "Physical",
            x = xmin - 5,
            y = Height / 3 / 2,
            fill = RedColor,
            text_anchor = "end",
            dominant_baseline = "middle",
            font_family = FontFamily,
            font_size = 18,
            font_weight = "normal",
            filter = "url(#dropShadow)"
        });
        svg.Children.Add(new text
        {
            content = "Emotional",
            x = xmin - 5,
            y = Height / 3 / 2 + Height / 3,
            fill = GreenColor,
            text_anchor = "end",
            dominant_baseline = "middle",
            font_family = FontFamily,
            font_size = 18,
            font_weight = "normal",
            filter = "url(#dropShadow)"
        });
        svg.Children.Add(new text
        {
            content = "Intellectual",
            x = xmin - 5,
            y = Height / 3 / 2 + 2 * Height / 3,
            fill = BlueColor,
            text_anchor = "end",
            dominant_baseline = "middle",
            font_family = FontFamily,
            font_size = 18,
            font_weight = "normal",
            filter = "url(#dropShadow)"
        });

        // draw text data labels
        svg.Children.Add(new text
        {
            content = $"{p:P0}",
            x = p * (xmax - xmin) + xmin + 10,
            y = Height / 3 / 2,
            fill = RedColor,
            text_anchor = "start",
            dominant_baseline = "middle",
            font_family = FontFamily,
            font_size = 20,
            font_weight = "normal",
            filter = "url(#dropShadow)"
        });
        svg.Children.Add(new text
        {
            content = $"{e:P0}",
            x = e * (xmax - xmin) + xmin + 10,
            y = Height / 3 / 2 + Height / 3,
            fill = GreenColor,
            text_anchor = "start",
            dominant_baseline = "middle",
            font_family = FontFamily,
            font_size = 20,
            font_weight = "normal",
            filter = "url(#dropShadow)"
        });
        svg.Children.Add(new text
        {
            content = $"{i:P0}",
            x = i * (xmax - xmin) + xmin + 10,
            y = Height / 3 / 2 + 2 * Height / 3,
            fill = BlueColor,
            text_anchor = "start",
            dominant_baseline = "middle",
            font_family = FontFamily,
            font_size = 20,
            font_weight = "normal",
            filter = "url(#dropShadow)"
        });

        return svg;
    }
}
