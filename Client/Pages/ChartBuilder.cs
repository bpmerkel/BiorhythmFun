﻿// <copyright file="ChartBuilder.cs" company="Brady P. Merkel">
// Copyright (c) Brady P. Merkel. All rights reserved.
// </copyright>

namespace BiorhythmFun.Client.Pages;

/// <summary>
/// ChartBuilder component to render an SVG Biorythm chart
/// </summary>
public class ChartBuilder : ComponentBase
{
    /// <summary>
    ///  Gets or sets the birthdate
    /// </summary>
    [Parameter] public DateTime BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the Start date
    /// </summary>
    [Parameter] public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the End date
    /// </summary>
    [Parameter] public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the Highlite date
    /// </summary>
    [Parameter] public DateTime HighliteDate { get; set; }

    /// <summary>
    /// Gets or sets the Chart name
    /// </summary>
    [Parameter] public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Chart size adjustment
    /// </summary>
    [Parameter] public int SizeAdjust { get; set; } = 0;

    public enum ChartType { Standard, GenderPrediction, BirthdatePrediction }

    /// <summary>
    /// Gets or sets the Chart Type
    /// </summary>
    [Parameter] public ChartType Type { get; set; } = ChartType.Standard;

    /// <summary>
    /// Handler for the chart hover event
    /// </summary>
    [Parameter] public EventCallback<ChartClickEventArgs> OnCycleClick { get { return onCycleClick; } set { onCycleClick = value; } }
    private static EventCallback<ChartClickEventArgs> onCycleClick;

    /// <summary>
    /// Handler for the chart hover event
    /// </summary>
    [Parameter] public EventCallback<(ChartBuilder, DateTime, int, int)> OnCycleHover { get { return onCycleHover; } set { onCycleHover = value; } }
    private static EventCallback<(ChartBuilder, DateTime, int, int)> onCycleHover;

    private const double twopi = 2d * Math.PI;
    private const string FontFamily = "arial";
    private const string ShadowColor = "#707070";
    private const string RedColor = "#FF0000";
    private const string BlueColor = "#0000FF";
    private const string GirlColor = "#f4c2c2";
    private const string BoyColor = "#89cff0";
    private static readonly string GreenColor = FromRgb(0, 128, 0);
    private static readonly string GoldColor = FromRgb(255, 215, 0);
    private static readonly string LighBlueColor = FromRgb(230, 230, 255);
    private static readonly string DarkBlueColor = FromRgb(0, 0, 140);
    private static readonly string GradientStartColor = FromRgb(102, 217, 255);
    private static readonly string GradientEndColor = FromRgb(179, 236, 255);

    private int Height = 200;
    private static int Daywidth = 25;
    private int daysinmonth;
    private int daysdiff;
    private int center;
    private int amp;
    private string id;

    /// <summary>
    /// Builds the render tree for the chart component.
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) => new SvgHelper().Render(Chart(), 0, builder);

    /// <summary>
    /// Registry of charts to allow for hover and click events
    /// </summary>
    private static readonly Dictionary<string, ChartBuilder> ChartRegistry = [];

    /// <summary>
    /// Generates a unique identifier for the chart based on its type and date range.
    /// </summary>
    /// <remarks>The generated identifier is composed of a prefix representing the chart type,  followed by
    /// formatted date values for the birth date, start date, and end date. The identifier is also registered in the
    /// chart registry for future reference.</remarks>
    /// <returns>A string representing the unique identifier for the chart.</returns>
    private string GenID()
    {
        var t = Type switch
        {
            ChartType.GenderPrediction => "G",
            ChartType.BirthdatePrediction => "B",
            _ => "S"
        };
        id = $"{t}{BirthDate:MMddyy}{StartDate:MMddyy}{EndDate:MMddyy}";
        ChartRegistry[id] = this;
        return id;
    }

    /// <summary>
    /// Draws the chart
    /// </summary>
    /// <returns></returns>
    private svg Chart()
    {
        Daywidth = 37 + SizeAdjust * 4;
        Height = Daywidth * 8;

        var svg = new svg
        {
            id = GenID(),
            height = Height,
            xmlns = "http://www.w3.org/2000/svg",
            onclick = GenerateOnclickCallback()
            //onmouseover = $"DotNet.invokeMethod('{Assembly.GetExecutingAssembly().GetName().Name}', '{nameof(IdentifyCycle)}', '{id}', evt.offsetX, evt.offsetY)",
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
            var (group, width) = DrawMonth(dt, offset == 0);
            if (offset > 0) group.transform = $"translate({offset})";
            svg.Children.Add(group);
            offset += width;
        }

        svg.width = offset;

        return svg;
    }

    /// <summary>
    /// Draws a month of the chart
    /// </summary>
    /// <param name="chartdate"></param>
    /// <param name="withAnimations"></param>
    /// <returns></returns>
    private (g, int) DrawMonth(DateTime chartdate, bool withAnimations)
    {
        daysinmonth = DateTime.DaysInMonth(chartdate.Year, chartdate.Month);
        daysdiff = Convert.ToInt32((chartdate.Date - BirthDate.Date).TotalDays) - 1;

        var width = daysinmonth * Daywidth;
        var group = new g { clip_path = $"url(#clipto{daysinmonth})" };

        group.Children.Add(DrawBackground(chartdate, withAnimations));

        var (pp, ppts) = DrawCycle(RedColor, 23, withAnimations);
        var (pe, epts) = DrawCycle(GreenColor, 28, withAnimations);
        var (pi, ipts) = DrawCycle(BlueColor, 33, withAnimations);
        var (gl, cpts) = LabelCycles(ppts, epts, ipts, withAnimations);

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

    /// <summary>
    /// Draws the background for the chart
    /// </summary>
    /// <param name="chartdate"></param>
    /// <param name="withAnimations"></param>
    /// <returns></returns>
    private g DrawBackground(DateTime chartdate, bool withAnimations)
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
                fill_opacity = withAnimations ? 0d : 1d,
                Children = withAnimations
                    ? new[]
                    {
                        new animate { id = $"{id}b{d}", attributeName = "fill-opacity", from = 0d, to = 1d, begin = d == 0 ? "0s" : $"{id}b{d-1}.end", dur = "50ms", repeatCount = "1" },
                    }
                    : null
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
                fill_opacity = withAnimations ? 0d : 1d,
                Children = withAnimations
                    ? new[]
                    {
                        new animate { attributeName = "fill-opacity", from = 0d, to = 1d, dur = "1.5s", repeatCount = "1" }
                    }
                    : null
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
                opacity = withAnimations ? 0d : 1d,
                Children = withAnimations
                    ? new[]
                    {
                        new animate { id = $"{id}day{d}", attributeName = "opacity", from = 0d, to = 1d, dur = ".1s", repeatCount = "1", begin = d == 0 ? "0s" : $"{id}day{d-1}.end" }
                    }
                    : null
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
                opacity = withAnimations ? 0d : 1d,
                Children = withAnimations
                    ? new[]
                    {
                        new animate { id = $"{id}mon{d}", attributeName = "opacity", from = 0d, to = 1d, dur = ".1s", repeatCount = "1", begin = $"{id}day{d}.end" }
                    }
                    : null
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
                opacity = withAnimations ? 0d : 1d,
                Children = withAnimations
                    ? new[]
                    {
                        new animate { attributeName = "opacity", from = 0d, to = 1d, dur = ".1s", repeatCount = "1", begin = $"{id}mon{d}.end" }
                    }
                    : null
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
            Children = withAnimations
                ? new[]
                {
                    new animate { attributeName = "width", from = 0d, to = daysinmonth * Daywidth, dur = "1.5s", repeatCount = "1" },
                }
                : null
        });

        group.Children.Add(new rect { x = 0, y = 0, width = daysinmonth * Daywidth, height = Height, fill = "none", stroke_width = 3, stroke = "#CCCCCC" });

        return group;
    }

    /// <summary>
    /// Draws a cycle line for the given color and cycle length
    /// </summary>
    /// <param name="color"></param>
    /// <param name="cycle"></param>
    /// <param name="withAnimations"></param>
    /// <returns></returns>
    private (path, IList<Point>) DrawCycle(string color, int cycle, bool withAnimations)
    {
        var diff = daysdiff - .5d; // offsets the sine wave so it looks nicer
        var coords = Enumerable.Range(-1, daysinmonth * 2 + 4)
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

        // compute the length of the curve
        var length = 0d;
        for (var i = 1; i < coords.Count; i++)
        {
            var p1 = coords[i - 1];
            var p2 = coords[i];
            length += Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }
        length = Math.Round(length, 2);

        var idx = $"{id}c{cycle}";
        var begin = cycle switch
        {
            23 => "1s",
            28 => $"{id}c23.begin+.5s",
            33 => $"{id}c28.begin+.5s",
            _ => "1s"
        };

        // draw a smooth curve
        var p = new path
        {
            d = $"M {coords[0].X},{coords[0].Y} L " + string.Join(" ", coords.Skip(1).Select(pt => $"{pt.X},{pt.Y}")),
            stroke = color,
            stroke_width = Daywidth / 3,
            fill = "none",
            filter = "url(#dropShadow)",
            stroke_dasharray = $"{length}",
            stroke_linecap = StrokeLinecap.round,
            stroke_dashoffset = 0d,
            Children = withAnimations
                ? new[]
                {
                    new animate { id = idx, attributeName = "stroke-dashoffset", from = length, to = 0d, begin = begin, dur = "1.5s", repeatCount = "1" },
                }
                : null
        };

        return (p, coords);
    }

    /// <summary>
    /// Generates the onclick callback for the chart
    /// </summary>
    /// <returns></returns>
    private string GenerateOnclickCallback()
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        var method = nameof(IdentifyDay);
        return $"DotNet.invokeMethod('{assemblyName}', '{method}', '{id}', evt.offsetX, evt.offsetY)";
    }

    /// <summary>
    /// Identifies the cycle based on the click coordinates and invokes the hover event callback.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    [JSInvokable]
    public static void IdentifyCycle(string id, int x, int y)
    {
        var instance = ChartRegistry[id];
        // compute the day based on the click x,y
        var days = x / Daywidth;
        var day = instance.StartDate.AddDays(days);
        onCycleHover.InvokeAsync((instance, day, x, y));
    }

    /// <summary>
    /// Identifies the day based on the click coordinates and invokes the click event callback.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>

    [JSInvokable]
    public static void IdentifyDay(string id, int x, int y)
    {
        var instance = ChartRegistry[id];
        // compute the day based on the click x,y
        var days = x / Daywidth;
        var day = instance.StartDate.AddDays(days);
        var diff = (day - instance.BirthDate).TotalDays;
        var args = new ChartClickEventArgs { Chart = instance, Date = day, X = x, Y = y, DaysDiff = Convert.ToInt32(diff) };

        // compute the chart status
        var physical = Math.Sin(diff / 23d * twopi);
        var emotional = Math.Sin(diff / 28d * twopi);
        var intellectual = Math.Sin(diff / 33d * twopi);
        args.Physical = physical < -.14d ? ChartClickEventArgs.CycleStatus.Negative
            : physical > .14d ? ChartClickEventArgs.CycleStatus.Positive
            : ChartClickEventArgs.CycleStatus.Critical;
        args.Emotional = emotional < -.01d ? ChartClickEventArgs.CycleStatus.Negative
            : emotional > .01d ? ChartClickEventArgs.CycleStatus.Positive
            : ChartClickEventArgs.CycleStatus.Critical;
        args.Intellectual = intellectual < -.10d ? ChartClickEventArgs.CycleStatus.Negative
            : intellectual > .10d ? ChartClickEventArgs.CycleStatus.Positive
            : ChartClickEventArgs.CycleStatus.Critical;

        onCycleClick.InvokeAsync(args);
    }

    /// <summary>
    /// Labels the cycles on the chart
    /// </summary>
    /// <param name="pp"></param>
    /// <param name="ep"></param>
    /// <param name="ip"></param>
    /// <param name="withAnimations"></param>
    /// <returns></returns>
    private (g, List<int>) LabelCycles(IList<Point> pp, IList<Point> ep, IList<Point> ip, bool withAnimations)
    {
        // locate the best place to draw the cycle labels
        var minyoffset = Convert.ToInt32(Daywidth * 1.25);
        const int minday = 1;
        var maxday = daysinmonth - 7;
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

        var pl = pcandidates.Count != 0
            ? pcandidates.OrderByDescending(e => e.Score).First().point
            : new Point { X = pp[1].X, Y = pp[1].Y };

        var el = ecandidates.Count != 0
            ? ecandidates.OrderByDescending(e => e.Score).First().point
            : new Point { X = ep[1].X, Y = ep[1].Y };

        var il = icandidates.Count != 0
            ? icandidates.OrderByDescending(e => e.Score).First().point
            : new Point { X = ip[1].X, Y = ip[1].Y };

        var clx = ccandidates.Count != 0
            ? ccandidates.OrderByDescending(e => e.Score).First().X
            : 2 * Daywidth;

        // draw the critical circles
        var group = new g();
        string priorid = null;
        foreach (var cc in criticals.Distinct())
        {
            var begin = priorid == null ? null : $"{priorid}.end";
            var idx = priorid = $"{id}crit{(cc < 0 ? -cc : cc)}";
            group.Children.Add(new circle
            {
                cx = cc,
                cy = center,
                r = Daywidth / 3d,
                fill = "none",
                stroke = GoldColor,
                stroke_width = withAnimations ? 1d : Daywidth / 5d,
                stroke_dasharray = $"{Daywidth * 2}",
                stroke_linecap = StrokeLinecap.round,
                opacity = withAnimations ? 0d : 1d,
                // filter = "url(#dropShadow)" // squares off corners too!
                Children = withAnimations
                    ? new[]
                    {
                        new animate { attributeName = "r", from = 0d, to = Daywidth / 3d, begin = begin, dur = "400ms", repeatCount = "1" },
                        new animate { attributeName = "stroke-width", from = 0d, to = Daywidth / 5d, begin = begin, dur = "400ms", repeatCount = "1", additive = "sum" },
                        new animate { id = idx, attributeName = "opacity", from = 0d, to = 1d, begin = begin, dur = "400ms", repeatCount = "1", additive = "sum" }
                    }
                    : null
            });
        }

        // Labels
        group.Children.Add(ShadowText("critical", clx, center, GoldColor, withAnimations));
        group.Children.Add(ShadowText("physical", pl.X + Daywidth, pl.Y, RedColor, withAnimations));
        group.Children.Add(ShadowText("emotional", el.X + Daywidth, el.Y, GreenColor, withAnimations));
        group.Children.Add(ShadowText("intellectual", il.X + Daywidth, il.Y, BlueColor, withAnimations));
        return (group, criticals.Distinct().ToList());
    }

    /// <summary>
    /// Draws the shadow text for the chart labels
    /// </summary>
    /// <param name="text"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    /// <param name="withAnimations"></param>
    /// <returns></returns>
    private static text ShadowText(string text, double x, double y, string color, bool withAnimations) => new()
    {
        x = x,
        y = y,
        dominant_baseline = "middle",
        font_family = FontFamily,
        font_size = Daywidth,
        content = text,
        fill = color,
        filter = "url(#dropShadow)",
        opacity = withAnimations ? 0d : 1d,
        Children = withAnimations
            ? new[]
            {
                new animate { attributeName = "x", from = 0d, to = x, dur = "2s", repeatCount = "1" },
                new animate { attributeName = "opacity", from = 0d, to = 1d, dur = "1.5s", repeatCount = "1", additive = "sum" }
            }
            : null
    };

    private static string FromRgb(int r, int g, int b) => $"#{r:X2}{g:X2}{b:X2}";
}

/// <summary>
/// Defines the event arguments for the chart click event.
/// </summary>
public class ChartClickEventArgs
{
    public enum CycleStatus { Positive, Negative, Critical }
    public ChartBuilder Chart { get; set; }
    public DateTime Date { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int DaysDiff { get; set; }
    public CycleStatus Physical { get; set; }
    public CycleStatus Emotional { get; set; }
    public CycleStatus Intellectual { get; set; }
}