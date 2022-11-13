// <copyright file="ChartBuilder.cs" company="Brady P. Merkel">
// Copyright (c) Brady P. Merkel. All rights reserved.
// </copyright>

namespace BioBlazor
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using BlazorSvgHelper;
	using BlazorSvgHelper.Classes.SubClasses;
	using Microsoft.AspNetCore.Components;
	using Microsoft.AspNetCore.Components.Rendering;

    public class ChartBuilder : ComponentBase
    {
        [Parameter]
        public DateTime Birthdate { get; set; }

        [Parameter]
        public DateTime Chartdate { get; set; }

        [Parameter]
        public double Height { get; set; }

        private svg svg = new svg();
        private bool simpleMode;
        private bool headerOnly;
        private int daysinmonth;
        private int daysdiff;
        private double width;
        private double center;
        private double amp;
        private double top;
        private DateTime chartstart;

        private const double daywidth = 20;
        private const double delta = .5;
        private const string fontFamily = "tahoma";
        private const string shadowColor = "#888888";
        private const string redColor = "#FF0000";
        private const string blueColor = "#0000FF";
        private const string transparent = "transparent";
        private static readonly string greenColor = fromRgb(0, 128, 0);
        private static readonly string goldColor = fromRgb(255, 215, 0);
        private static readonly string lighBlueColor = fromRgb(230, 230, 255);
        private static readonly string darkBlueColor = fromRgb(0, 0, 140);
        private static readonly string gradientStartColor = fromRgb(102, 217, 255);
        private static readonly string gradientEndColor = fromRgb(179, 236, 255);

        private svg chart()
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            new SvgHelper().Cmd_Render(chart(), 0, builder);
        }

        {
            chartstart = new DateTime(Chartdate.Year, Chartdate.Month, 1);
            daysinmonth = DateTime.DaysInMonth(chartstart.Year, chartstart.Month);
            daysdiff = Convert.ToInt32((chartstart.Date - Birthdate.Date).TotalDays) - 1;

            width = daysinmonth * daywidth;
            top = (headerOnly || simpleMode) ? 0 : daywidth;
            center = (Height - top) / 2 + (simpleMode ? 0 : daywidth * 2);
            amp = (Height - top) / 2 - (simpleMode ? daywidth : daywidth * 1.5);

            svg = new svg
            {
                width = width,
                height = Height,
                id = "biorhythm",
                xmlns = "http://www.w3.org/2000/svg"
            };

            var _defs1 = new defs();
            var linear1 = new linearGradient { id = "grad1" };
            linear1.Children.Add(new stop { stop_color = gradientStartColor, offset = "0" });
            linear1.Children.Add(new stop { stop_color = gradientEndColor, offset = "1" });
            _defs1.Children.Add(linear1);

            var filter = new filter { id = "dropShadow" };
            filter.Children.Add(new feDropShadow { dx = .5d, dy = .5d, stdDeviation = .7d, flood_color = shadowColor, flood_opacity = 1d });
            _defs1.Children.Add(filter);

            svg.Children.Add(_defs1);

            drawBackground();

            if (!headerOnly)
            {
                var ppts = drawCycle(redColor, 23);
                var epts = drawCycle(greenColor, 28);
                var ipts = drawCycle(blueColor, 33);
                labelCycles(ppts, epts, ipts);
            }

            return svg;
        }

        private void drawBackground()
        {
            if (!simpleMode || headerOnly)
            {
                // see https://stackoverflow.com/questions/5546346/how-to-place-and-center-text-in-an-svg-rectangle
                svg.Children.Add(new text
                {
                    content = $"{chartstart:MMMM} {chartstart:yyyy}",
                    x = 0,
                    y = daywidth * 2 / 3,
                    fill = darkBlueColor,
                    //text_anchor = "middle",
                    //dominant_baseline = "middle",
                    font_family = fontFamily,
                    font_size = daywidth * .9,
                    font_weight = "normal",
                    filter = "url(#dropShadow)"
                });
            }

            // draw the day grid
            for (var d = 0; d < daysinmonth; d++)
            {
                svg.Children.Add(new rect { width = daywidth, height = Height, x = d * daywidth, y = top, fill = "url(#grad1)", stroke_width = 0 });
            }

            // draw a month divider hint line
            svg.Children.Add(new rect { width = 1, height = Height, fill = darkBlueColor, x = 0, y = top });

            // color the chart date only in the right month
            if (chartstart.Year == Chartdate.Year && chartstart.Month == Chartdate.Month)
            {
                svg.Children.Add(new rect { width = daywidth, height = Height, fill = lighBlueColor, x = daywidth * (Chartdate.Day - 1), y = top });
            }

            if (!simpleMode || headerOnly)
            {
                // draw the day numbers
                var day = chartstart;
                for (var d = 0; d < daysinmonth; d++)
                {
                    svg.Children.Add(new text
                    {
                        content = $"{day:ddd}",
                        x = d * daywidth + daywidth / 2,
                        y = top + daywidth / 2,
                        fill = darkBlueColor,
                        text_anchor = "middle",
                        dominant_baseline = "middle",
                        font_family = fontFamily,
                        font_size = daywidth / 2,
                        font_weight = "normal"
                    });
                    svg.Children.Add(new text
                    {
                        content = $"{day:MMM}",
                        x = d * daywidth + daywidth / 2,
                        y = top + daywidth,
                        fill = darkBlueColor,
                        text_anchor = "middle",
                        dominant_baseline = "middle",
                        font_family = fontFamily,
                        font_size = daywidth / 2,
                        font_weight = "normal"
                    });
                    svg.Children.Add(new text
                    {
                        content = $"{day.Day}",
                        x = d * daywidth + daywidth / 2,
                        y = top + daywidth * 1.5,
                        fill = darkBlueColor,
                        text_anchor = "middle",
                        dominant_baseline = "middle",
                        font_family = fontFamily,
                        font_size = daywidth / 2,
                        font_weight = "normal"
                    });
                    day = day.AddDays(1);
                }
            }

            if (!headerOnly)
            {
                // draw the critical line (note: can't use dropshadow filter on 'line' element
                svg.Children.Add(new rect
                {
                    x = 0,
                    y = center - daywidth / 8,
                    width = width,
                    height = daywidth / 4,
                    fill = goldColor,
                    stroke_width = 0,
                    filter = "url(#dropShadow)"
                });
            }
        }

        private IList<point> drawCycle(string color, int cycle)
        {
            var diff = daysdiff - .5; // the .5 offsets the sin wave half a day so it looks nicer
            var coords = new List<point>(daysinmonth + 2);
            for (var j = -1d; j <= daysinmonth + 1; j += delta, diff += delta)
            {
                coords.Add(new point { x = j * daywidth, y = center - amp * Math.Sin((diff / cycle) * Math.PI * 2) });
            }

            // draw a smooth curve
            var sb = new StringBuilder($"M {coords[0].x:f0} {coords[1].y:f0}");
            foreach (var pt in coords.Skip(1))
            {
                sb.Append($" L {pt.x:f0} {pt.y:f0}");
            }

            //var bgs = new linearGradient
            //{
            //    //StartPoint = new Point( 0, simpleMode ? 0 : daywidth * 2 ),
            //    StartPoint = new Point(0, top),
            //    EndPoint = new Point(0, height),
            //    Opacity = 1.0,
            //    ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
            //    MappingMode = BrushMappingMode.Absolute,
            //    SpreadMethod = GradientSpreadMethod.Pad
            //};
            //bgs.GradientStops.Add(new GradientStop { Offset = 0.0, Color = color });
            //bgs.GradientStops.Add(new GradientStop { Offset = 0.45, Color = color });
            //bgs.GradientStops.Add(new GradientStop { Offset = 0.5, Color = _goldColor });
            //bgs.GradientStops.Add(new GradientStop { Offset = 0.55, Color = color });
            //bgs.GradientStops.Add(new GradientStop { Offset = 1.0, Color = color });

            svg.Children.Add(new path
            {
                d = sb.ToString(),
                stroke = color,
                stroke_width = daywidth / 3,
                fill = transparent,
                filter = "url(#dropShadow)"
            });

            return coords;
        }

        private void labelCycles(IList<point> pp, IList<point> ep, IList<point> ip)
        {
            // locate the best place to draw the cycle labels
            var clx = 0d;
            var plx = 0d;
            var elx = 0d;
            var ilx = 0d;
            var ply = 0d;
            var ely = 0d;
            var ily = 0d;
            const double minyoffset = daywidth * 2;
            const int minday = 3;
            var maxday = pp.Count - 6;
            var criticals = new List<double>();

            for (var d = 1; d < pp.Count; ++d)
            {
                var p0 = pp[d - 1].y;
                var e0 = ep[d - 1].y;
                var i0 = ip[d - 1].y;
                var p1 = pp[d].y;
                var e1 = ep[d].y;
                var i1 = ip[d].y;

                if (d >= minday && d <= maxday && plx == 0d
                    && Math.Abs(p1 - e1) > minyoffset
                    && Math.Abs(p1 - i1) > minyoffset
                    && Math.Abs(p1 - center) > minyoffset
                    && Math.Abs(p1 - ely) > minyoffset
                    && Math.Abs(p1 - ily) > minyoffset)
                {
                    plx = pp[d].x;
                    ply = p1;
                }

                if (d >= minday && d <= maxday && elx == 0d
                    && Math.Abs(e1 - p1) > minyoffset
                    && Math.Abs(e1 - i1) > minyoffset
                    && Math.Abs(e1 - center) > minyoffset
                    && Math.Abs(e1 - ply) > minyoffset
                    && Math.Abs(e1 - ily) > minyoffset)
                {
                    elx = ep[d].x;
                    ely = e1;
                }

                if (d >= minday && d <= maxday && ilx == 0d
                    && Math.Abs(i1 - p1) > minyoffset
                    && Math.Abs(i1 - e1) > minyoffset
                    && Math.Abs(i1 - center) > minyoffset
                    && Math.Abs(i1 - ply) > minyoffset
                    && Math.Abs(i1 - ely) > minyoffset)
                {
                    ilx = ip[d].x;
                    ily = i1;
                }

                if (d >= minday && d <= maxday && clx == 0
                    && Math.Abs(p1 - center) > minyoffset * 2
                    && Math.Abs(e1 - center) > minyoffset * 2
                    && Math.Abs(i1 - center) > minyoffset * 2)
                {
                    clx = d * daywidth * delta;
                }

                // find critical dates
                var ps0 = Math.Sign(Math.Truncate(p0) - Math.Truncate(center));
                var ps1 = Math.Sign(Math.Truncate(p1) - Math.Truncate(center));
                // 1 - 0 = 1
                // -1 - 0 = -1
                // 0 - 1 = -1
                // 1 - -1 = 2
                // -1 - 1 = -2
                //if (pc == -1 && p0 < center) criticals.Add(py[d].x);
                //else if (pc == -1 && p0 >= center) criticals.Add(py[d - 1].x);
                //else if (pc == 1) criticals.Add(py[d].x);
                //else if (pc == 2 || pc == -2)
                if (ps0 != ps1)
                {
                    if (ps0 == 0) criticals.Add(pp[d - 1].x);
                    else if (ps1 == 0) criticals.Add(pp[d].x);
                    else criticals.Add((pp[d].x + pp[d - 1].x) / 2d);
                }

                if (Math.Sign(e0 - center) != Math.Sign(e1 - center))
                    criticals.Add((ep[d].x + ep[d - 1].x) / 2d);

                if (Math.Sign(i0 - center) != Math.Sign(i1 - center))
                    criticals.Add((ip[d].x + ip[d - 1].x) / 2d);
            }

            if (plx == 0)
            {
                plx = pp[1].x;
                ply = pp[1].y;
            }
            if (elx == 0)
            {
                elx = ep[1].x;
                ely = ep[1].y;
            }
            if (ilx == 0)
            {
                ilx = ip[1].x;
                ily = ip[1].y;
            }
            if (clx == 0)
            {
                clx = 1.5 * daywidth;
            }

            // draw the critical circles
            foreach (var cc in criticals.Distinct())
            {
                svg.Children.Add(new circle
                {
                    cx = cc,
                    cy = center,
                    r = daywidth / 3,
                    fill = transparent,
                    stroke = goldColor,
                    stroke_width = 5
                    // filter = "url(#dropShadow)" // squares off corners too!
                });
            }

            // Text
            if (!simpleMode)
            {
                shadowText("critical", clx, center - daywidth, goldColor);
                shadowText("physical", plx + daywidth, ply, redColor);
                shadowText("emotional", elx + daywidth, ely, greenColor);
                shadowText("intellectual", ilx + daywidth, ily, blueColor);
            }
        }

        private void shadowText(string text, double x, double y, string color)
        {
            svg.Children.Add(new text
            {
                x = x,
                y = y,
                //TextAlignment = TextAlignment.Left,
                font_family = fontFamily,
                font_size = daywidth,
                content = text,
                //VerticalAlignment = VerticalAlignment.Top,
                //Padding = new Thickness(0),
                fill = color,
                filter = "url(#dropShadow)"
            });
        }

        private static string fromRgb(int r, int g, int b) => $"#{r:X2}{g:X2}{b:X2}";
        private class point { public double x; public double y; }
    }
}