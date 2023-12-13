// <copyright file="ChartBuilder.cs" company="Brady P. Merkel">
// Copyright (c) Brady P. Merkel. All rights reserved.
// </copyright>

namespace BiorhythmFun.Client.Pages;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BiorthymFun.Client.Svg;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

/// <summary>
/// ChartBuilder component to render an SVG Biorythm chart
/// </summary>
public class ChartBuilder : ComponentBase
{
    private int daysinmonth;
    private int daysdiff;
    private int center;
    private int amp;

    private const int Daywidth = 25;
    private const string FontFamily = "arial";
    private const string ShadowColor = "#707070";
    private const string RedColor = "#FF0000";
    private const string BlueColor = "#0000FF";
    private const string GirlColor = "#f4c2c2";
    private const string BoyColor = "#89cff0";
    private const string Transparent = "transparent";
    private static readonly string GreenColor = FromRgb(0, 128, 0);
    private static readonly string GoldColor = FromRgb(255, 215, 0);
    private static readonly string LighBlueColor = FromRgb(230, 230, 255);
    private static readonly string DarkBlueColor = FromRgb(0, 0, 140);
    private static readonly string GradientStartColor = FromRgb(102, 217, 255);
    private static readonly string GradientEndColor = FromRgb(179, 236, 255);

    /// <summary>
    ///  Gets or sets the birthdate
    /// </summary>
    [Parameter]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the Start date
    /// </summary>
    [Parameter]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the End date
    /// </summary>
    [Parameter]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the Highlite date
    /// </summary>
    [Parameter]
    public DateTime HighliteDate { get; set; }

    /// <summary>
    /// Gets or sets the Height
    /// </summary>
    [Parameter]
    public int Height { get; set; }

    public enum ChartType { Standard, GenderPrediction, BirthdatePrediction }

    /// <summary>
    /// Gets or sets the Chart Type
    /// </summary>
    [Parameter]
    public ChartType Type { get; set; } = ChartType.Standard;

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder) => new SvgHelper().Render(Chart(), 0, builder);

    private svg Chart()
    {
        var svg = new svg
        {
            id = DateTime.Now.ToString("HHmmssffff"),
            height = Height,
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

        // add clip paths for the possible days in month
        foreach (var mlen in new[] { 28, 29, 30, 31 })
        {
            var clip = new clipPath { id = $"clipto{mlen}" };
            clip.Children.Add(new rect { x = 0, y = 0, width = mlen * Daywidth, height = Height });
            defs1.Children.Add(clip);
        }

        svg.Children.Add(defs1);

        center = Height / 2 + Daywidth;
        amp = Convert.ToInt32(Height / 2 - Daywidth * 1.5);

        var offset = 0;
        for (var dt = StartDate; dt < EndDate; dt = dt.AddMonths(1))
        {
            var (group, width) = DrawMonth(dt);
            if (offset > 0) group.transform = $"translate({offset})";
            svg.Children.Add(group);
            offset += width;
        }

        svg.width = offset;

        return svg;
    }

    private (g, int) DrawMonth(DateTime chartdate)
    {
        daysinmonth = DateTime.DaysInMonth(chartdate.Year, chartdate.Month);
        daysdiff = Convert.ToInt32((chartdate.Date - BirthDate.Date).TotalDays) - 1;

        var width = daysinmonth * Daywidth;
        var group = new g { clip_path = $"url(#clipto{daysinmonth})" };

        group.Children.Add(DrawBackground(chartdate));

        var (pp, ppts) = DrawCycle(RedColor, 23);
        var (pe, epts) = DrawCycle(GreenColor, 28);
        var (pi, ipts) = DrawCycle(BlueColor, 33);
        var (gl, cpts) = LabelCycles(ppts, epts, ipts);

        if (Type == ChartType.GenderPrediction)
        {
            var top = center - amp;
            var height = amp * 2;
            // get P and E and highlite Blue or Pink days to indicate Boy or Girl
            ppts
                .Select((p, i) => (p, i))
                .Where(a => a.p.Y >= center && epts[a.i].Y <= center)
                .Select(a => a.p.X)
                .ToList()
                .ForEach(g => group.Children.Add(new rect { width = Daywidth, height = height, x = g, y = top, fill = GirlColor, fill_opacity = .5 }));
            ppts
                .Select((p, i) => (p, i))
                .Where(a => a.p.Y <= center && epts[a.i].Y >= center)
                .Select(a => a.p.X)
                .ToList()
                .ForEach(b => group.Children.Add(new rect { width = Daywidth, height = height, x = b, y = top, fill = BoyColor, fill_opacity = .5 }));
            var x = (HighliteDate - StartDate).TotalDays * Daywidth;
            group.Children.Add(new rect { width = Daywidth, height = height, x = x, y = top, fill = GoldColor, fill_opacity = .9 });
        }
        else if (Type == ChartType.BirthdatePrediction)
        {
            // cycle through critical days and highlite likely birth days
            var top = center - amp;
            var height = amp * 2;
            cpts
                .Where(x => Math.Abs((HighliteDate - StartDate.AddDays(x / Daywidth)).TotalDays) <= 7)   // 7 days +/- of expected birthdate
                .ToList()
                .ForEach(x => group.Children.Add(new rect { width = Daywidth, height = height, x = x - Daywidth / 2, y = top, fill = GoldColor, fill_opacity = .5 }));
            var x = (HighliteDate - StartDate).TotalDays * Daywidth;
            group.Children.Add(new rect { width = Daywidth, height = height, x = x, y = top, fill = GoldColor, fill_opacity = .9 });
        }

        group.Children.Add(pp);
        group.Children.Add(pe);
        group.Children.Add(pi);
        group.Children.Add(gl);
        return (group, width);
    }

    private g DrawBackground(DateTime chartdate)
    {
        var group = new g();

        // draw the day grid
        for (var d = 0; d < daysinmonth; d++)
        {
            group.Children.Add(new rect
            {
                width = Daywidth,
                height = Height,
                x = d * Daywidth,
                y = 0,
                fill = "url(#grad1)",
                stroke_width = 0,
                fill_opacity = 0d,
                Children = new[]
                {
                    new animate { id = $"b{d}", attributeName = "fill-opacity", from = 0d, to = 1d, begin = d == 0 ? "0s" : $"b{d-1}.end", dur = "50ms", repeatCount = "1" },
                }
            });
        }

        // color the chart date only in the right month
        if (chartdate.Year == DateTime.Today.Year && chartdate.Month == DateTime.Today.Month)
        {
            group.Children.Add(new rect
            {
                width = Daywidth,
                height = Height,
                fill = LighBlueColor,
                x = Daywidth * (DateTime.Today.Day - 1),
                y = 0,
                fill_opacity = 0d,
                Children = new[]
                {
                    new animate { attributeName = "fill-opacity", from = 0d, to = 1d, dur = "1.5s", repeatCount = "1" }
                }
            });
        }

        // draw the day numbers
        var day = chartdate;
        for (var d = 0; d < daysinmonth; d++)
        {
            group.Children.Add(new text
            {
                content = $"{day:ddd}",
                x = d * Daywidth + Daywidth / 2,
                y = Daywidth / 2,
                fill = DarkBlueColor,
                text_anchor = "middle",
                dominant_baseline = "middle",
                font_family = FontFamily,
                font_size = Daywidth / 2,
                font_weight = "normal",
                opacity = 0d,
                Children = new[]
                {
                    new animate { id = $"day{d}", attributeName = "opacity", from = 0d, to = 1d, dur = ".1s", repeatCount = "1", begin = d == 0 ? "0s" : $"day{d-1}.end" }
                }
            });
            group.Children.Add(new text
            {
                content = $"{day:MMM}",
                x = d * Daywidth + Daywidth / 2,
                y = Daywidth,
                fill = DarkBlueColor,
                text_anchor = "middle",
                dominant_baseline = "middle",
                font_family = FontFamily,
                font_size = Daywidth / 2,
                font_weight = "normal",
                opacity = 0d,
                Children = new[]
                {
                    new animate { id = $"mon{d}", attributeName = "opacity", from = 0d, to = 1d, dur = ".1s", repeatCount = "1", begin = $"day{d}.end" }
                }
            });
            group.Children.Add(new text
            {
                content = $"{day.Day}",
                x = d * Daywidth + Daywidth / 2,
                y = Daywidth * 1.5,
                fill = DarkBlueColor,
                text_anchor = "middle",
                dominant_baseline = "middle",
                font_family = FontFamily,
                font_size = Daywidth / 2,
                font_weight = "normal",
                opacity = 0d,
                Children = new[]
                {
                    new animate { attributeName = "opacity", from = 0d, to = 1d, dur = ".1s", repeatCount = "1", begin = $"mon{d}.end" }
                }
            });
            day = day.AddDays(1);
        }

        // draw the critical line (note: can't use dropshadow filter on 'line' element)
        group.Children.Add(new rect
        {
            x = 0,
            y = center - Daywidth / 8,
            width = daysinmonth * Daywidth,
            height = Daywidth / 4,
            fill = GoldColor,
            stroke_width = 0,
            filter = "url(#dropShadow)",
            Children = new[]
            {
                new animate { attributeName = "width", from = 0d, to = daysinmonth * Daywidth, dur = "1.5s", repeatCount = "1" },
            }
        });

        group.Children.Add(new rect { x = 0, y = 0, width = daysinmonth * Daywidth, height = Height, fill = Transparent, stroke_width = 3, stroke = "#CCCCCC" });

        return group;
    }

    private (path, IList<Point>) DrawCycle(string color, int cycle)
    {
        const double twopi = 2d * Math.PI;
        var diff = daysdiff - .5d; // offsets the sine wave so it looks nicer
        var coords = Enumerable.Range(-1, daysinmonth * 2 + 3)
            .Select(j =>
            {
                diff += .5d;
                return new Point
                {
                    X = Convert.ToInt32(j * Daywidth / 2d),
                    Y = Convert.ToInt32(center - amp * Math.Sin(diff / cycle * twopi))
                };
            })
            .ToList();

        // draw a smooth curve
        var length = (Daywidth + 4) * daysinmonth;
        var id = $"c{cycle}";
        var begin = cycle switch
        {
            23 => "1s",
            28 => "c23.begin+.5s",
            33 => "c28.begin+.5s"
        };

        var p = new path
        {
            d = $"M {coords[0].X},{coords[0].Y} L " + string.Join(" ", coords.Skip(1).Select(pt => $"{pt.X},{pt.Y}")),
            stroke = color,
            stroke_width = Daywidth / 3,
            fill = Transparent,
            filter = "url(#dropShadow)",
            stroke_dasharray = $"{length}",
            stroke_linecap = StrokeLinecap.round,
            stroke_dashoffset = length,
            Children = new[]
            {
                new animate { id = id, attributeName = "stroke-dashoffset", from = length, to = 0, begin = begin, dur = "1.5s", repeatCount = "1" },
            }
        };

        return (p, coords);
    }

    private (g, List<int>) LabelCycles(IList<Point> pp, IList<Point> ep, IList<Point> ip)
    {
        // locate the best place to draw the cycle labels
        var minyoffset = Convert.ToInt32(Daywidth * 1.25);
        const int minday = 1;
        var maxday = pp.Count - 14;
        var pcandidates = new List<(Point point, int Score)>();
        var ecandidates = new List<(Point point, int Score)>();
        var icandidates = new List<(Point point, int Score)>();
        var ccandidates = new List<(int X, int Score)>();
        var criticals = new List<int>();

        for (var d = 1; d < pp.Count; ++d)
        {
            var p0 = pp[d - 1].Y;
            var e0 = ep[d - 1].Y;
            var i0 = ip[d - 1].Y;
            var p1 = pp[d].Y;
            var e1 = ep[d].Y;
            var i1 = ip[d].Y;

            if (d >= minday && d <= maxday)
            {
                if (Math.Abs(p1 - e1) > minyoffset
                    && Math.Abs(p1 - i1) > minyoffset
                    && Math.Abs(p1 - center) > minyoffset)
                {
                    pcandidates.Add((new Point { X = pp[d].X, Y = p1 }, Math.Abs(p1 - e1) + Math.Abs(p1 - i1) + Math.Abs(p1 - center)));
                }

                if (Math.Abs(e1 - p1) > minyoffset
                    && Math.Abs(e1 - i1) > minyoffset
                    && Math.Abs(e1 - center) > minyoffset)
                {
                    ecandidates.Add((new Point { X = ep[d].X, Y = e1 }, Math.Abs(e1 - p1) + Math.Abs(e1 - i1) + Math.Abs(e1 - center)));
                }

                if (Math.Abs(i1 - p1) > minyoffset
                    && Math.Abs(i1 - e1) > minyoffset
                    && Math.Abs(i1 - center) > minyoffset)
                {
                    icandidates.Add((new Point { X = ip[d].X, Y = i1 }, Math.Abs(i1 - p1) + Math.Abs(i1 - e1) + Math.Abs(i1 - center)));
                }

                if (Math.Abs(p1 - center) > minyoffset
                    && Math.Abs(e1 - center) > minyoffset
                    && Math.Abs(i1 - center) > minyoffset)
                {
                    ccandidates.Add((ip[d].X, Math.Abs(p1 - center) + Math.Abs(e1 - center) + Math.Abs(i1 - center))); // use x of physical rhythm here
                }
            }

            // find critical dates
            if (p0 == center) criticals.Add(pp[d - 1].X);
            else if (p1 == center) criticals.Add(pp[d].X);
            else if (Math.Sign(p0 - center) != Math.Sign(p1 - center))
                criticals.Add((pp[d - 1].X + pp[d].X) / 2);

            if (e0 == center) criticals.Add(ep[d - 1].X);
            else if (e1 == center) criticals.Add(ep[d].X);
            else if (Math.Sign(e0 - center) != Math.Sign(e1 - center))
                criticals.Add((ep[d - 1].X + ep[d].X) / 2);

            if (i0 == center) criticals.Add(ip[d - 1].X);
            else if (i1 == center) criticals.Add(ip[d].X);
            else if (Math.Sign(i0 - center) != Math.Sign(i1 - center))
                criticals.Add((ip[d - 1].X + ip[d].X) / 2);
        }

        var pl = pcandidates.Any()
            ? pcandidates.OrderByDescending(e => e.Score).First().point
            : new Point { X = pp[1].X, Y = pp[1].Y };

        var el = ecandidates.Any()
            ? ecandidates.OrderByDescending(e => e.Score).First().point
            : new Point { X = ep[1].X, Y = ep[1].Y };

        var il = icandidates.Any()
            ? icandidates.OrderByDescending(e => e.Score).First().point
            : new Point { X = ip[1].X, Y = ip[1].Y };

        var clx = ccandidates.Any()
            ? ccandidates.OrderByDescending(e => e.Score).First().X
            : 2 * Daywidth;

        // draw the critical circles
        var group = new g();
        foreach (var cc in criticals.Distinct())
        {
            var c = new circle
            {
                cx = cc,
                cy = center,
                r = Daywidth / 3d,
                fill = Transparent,
                stroke = GoldColor,
                stroke_width = 5,
                stroke_dasharray = "50.265",
                stroke_linecap = StrokeLinecap.round,
                opacity = 0d,
                // filter = "url(#dropShadow)" // squares off corners too!
                Children = new[]
                {
                    new animate { attributeName = "r", from = 0d, to = Daywidth / 3d, dur = "1.5s", repeatCount = "1" },
                    new animate { attributeName = "stroke-width", from = 0d, to = 1d, dur = "1.5s", repeatCount = "1", additive = "sum" },
                    new animate { attributeName = "opacity", from = 0d, to = 1d, dur = "1.5s", repeatCount = "1", additive = "sum" }
                }
            };

            //c.Children.Add(new animate { attributeName = "stroke-dashoffset", values = "50.265;0", dur = .75, repeatCount = "3" });

            group.Children.Add(c);
        }

        // Labels
        group.Children.Add(ShadowText("critical", clx, center, GoldColor));
        group.Children.Add(ShadowText("physical", pl.X + Daywidth, pl.Y, RedColor));
        group.Children.Add(ShadowText("emotional", el.X + Daywidth, el.Y, GreenColor));
        group.Children.Add(ShadowText("intellectual", il.X + Daywidth, il.Y, BlueColor));
        return (group, criticals.Distinct().ToList());
    }

    private static text ShadowText(string text, double x, double y, string color) => new()
    {
        x = x,
        y = y,
        dominant_baseline = "middle",
        font_family = FontFamily,
        font_size = Daywidth,
        content = text,
        fill = color,
        filter = "url(#dropShadow)",
        opacity = 0d,
        Children = new[]
        {
            new animate { attributeName = "x", from = 0d, to = x, dur = "2s", repeatCount = "1" },
            new animate { attributeName = "opacity", from = 0d, to = 1d, dur = "1.5s", repeatCount = "1", additive = "sum" }
        }
    };

    private static string FromRgb(int r, int g, int b) => $"#{r:X2}{g:X2}{b:X2}";
}