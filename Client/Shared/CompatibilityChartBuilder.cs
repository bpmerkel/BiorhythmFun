// <copyright file="CompatibilityChartBuilder.cs" company="Brady P. Merkel">
// Copyright (c) Brady P. Merkel. All rights reserved.
// </copyright>

namespace BiorhythmFun.Client;

using System;
using System.Drawing;
using BiorthymFun.Client.Svg;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

/// <summary>
/// ChartBuilder control.
/// </summary>
public class CompatibilityChartBuilder : ComponentBase
{
    private const string FontFamily = "tahoma";
    private const string ShadowColor = "#707070";
    private const string RedColor = "#FF0000";
    private const string DarkRedColor = "#aa0000";
    private const string BlueColor = "#0000FF";
    private const string DarkBlueColor = "#0000aa";
    private static readonly string GreenColor = FromRgb(0, 128, 0);
    private static readonly string DarkGreenColor = FromRgb(0, 96, 0);
    private static readonly string GradientStartColor = FromRgb(102, 217, 255);
    private static readonly string GradientEndColor = FromRgb(179, 236, 255);

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
    protected override void BuildRenderTree(RenderTreeBuilder builder) => new SvgHelper().Cmd_Render(Chart(), 0, builder);

    private static text ShadowText(string text, double x, double y, string color) => new()
    {
        x = x,
        y = y,
        dominant_baseline = "middle",
        font_family = FontFamily,
        font_size = 25,
        content = text,
        fill = color,
        filter = "url(#dropShadow)"
    };

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
        var linear1 = new linearGradient { id = "grad1" };
        linear1.Children.Add(new stop { stop_color = GradientStartColor, offset = "0" });
        linear1.Children.Add(new stop { stop_color = GradientEndColor, offset = "1" });
        defs1.Children.Add(linear1);

        var filter = new filter { id = "dropShadow" };
        filter.Children.Add(new feDropShadow { dx = .5d, dy = .5d, stdDeviation = .7d, flood_color = ShadowColor, flood_opacity = 1d });
        defs1.Children.Add(filter);

        svg.Children.Add(defs1);

        var daysdiff = Math.Abs((Birthdate1.Date - Birthdate2.Date).TotalDays);
        var p = 1d - (daysdiff % 23d) / 23d;
        var e = 1d - (daysdiff % 28d) / 28d;
        var i = 1d - (daysdiff % 33d) / 33d;

        var group = new g();

        // draw frame and background
        group.Children.Add(new rect { width = Width, height = Height, x = 0, y = 0, fill = "url(#grad1)", stroke_width = 3, stroke = DarkBlueColor });

        // draw bars
        group.Children.Add(new rect { width = p * (Width - 100), height = Height / 3 - 20, fill = RedColor, x = 100, y = 10, stroke_width = 1, stroke = DarkRedColor, filter = "url(#dropShadow)" });
        group.Children.Add(new rect { width = e * (Width - 100), height = Height / 3 - 20, fill = GreenColor, x = 100, y = Height / 3 + 10, stroke_width = 1, stroke = DarkGreenColor, filter = "url(#dropShadow)" });
        group.Children.Add(new rect { width = i * (Width - 100), height = Height / 3 - 20, fill = BlueColor, x = 100, y = 2 * Height / 3 + 10, stroke_width = 1, stroke = DarkBlueColor, filter = "url(#dropShadow)" });

        // draw text labels Physical, Emotional, Intellectual
        group.Children.Add(new text
        {
            content = "Physical",
            x = 90,
            y = Height / 3 / 2,
            fill = RedColor,
            text_anchor = "end",
            dominant_baseline = "middle",
            font_family = FontFamily,
            font_size = 18,
            font_weight = "normal",
            filter = "url(#dropShadow)"
        });
        group.Children.Add(new text
        {
            content = "Emotional",
            x = 90,
            y = Height / 3 / 2 + Height / 3,
            fill = GreenColor,
            text_anchor = "end",
            dominant_baseline = "middle",
            font_family = FontFamily,
            font_size = 18,
            font_weight = "normal",
            filter = "url(#dropShadow)"
        });
        group.Children.Add(new text
        {
            content = "Intellectual",
            x = 90,
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
        group.Children.Add(new text
        {
            content = $"{p:P0}",
            x = p * (Width - 100) + 110,
            y = Height / 3 / 2,
            fill = RedColor,
            text_anchor = "left",
            dominant_baseline = "middle",
            font_family = FontFamily,
            font_size = 20,
            font_weight = "normal",
            filter = "url(#dropShadow)"
        });
        group.Children.Add(new text
        {
            content = $"{e:P0}",
            x = e * (Width - 100) + 110,
            y = Height / 3 / 2 + Height / 3,
            fill = GreenColor,
            text_anchor = "left",
            dominant_baseline = "middle",
            font_family = FontFamily,
            font_size = 20,
            font_weight = "normal",
            filter = "url(#dropShadow)"
        });
        group.Children.Add(new text
        {
            content = $"{i:P0}",
            x = i * (Width - 100) + 110,
            y = Height / 3 / 2 + 2 * Height / 3,
            fill = BlueColor,
            text_anchor = "left",
            dominant_baseline = "middle",
            font_family = FontFamily,
            font_size = 20,
            font_weight = "normal",
            filter = "url(#dropShadow)"
        });

        svg.Children.Add(group);
        return svg;
    }
}
