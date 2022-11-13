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
        public int Height { get; set; }

        private svg svg = new svg();
        private int daysinmonth;
        private int daysdiff;
        private int width;
        private int center;
        private int amp;
        private DateTime chartstart;

        private const int Daywidth = 20;
        private const string FontFamily = "tahoma";
        private const string ShadowColor = "#888888";
        private const string RedColor = "#FF0000";
        private const string BlueColor = "#0000FF";
        private const string Transparent = "transparent";
        private static readonly string GreenColor = FromRgb(0, 128, 0);
        private static readonly string GoldColor = FromRgb(255, 215, 0);
        private static readonly string LighBlueColor = FromRgb(230, 230, 255);
        private static readonly string DarkBlueColor = FromRgb(0, 0, 140);
        private static readonly string GradientStartColor = FromRgb(102, 217, 255);
        private static readonly string GradientEndColor = FromRgb(179, 236, 255);

        /// <inheritdoc/>
        protected override void BuildRenderTree(RenderTreeBuilder builder) => new SvgHelper().Cmd_Render(Chart(), 0, builder);

        private static string FromRgb(int r, int g, int b) => $"#{r:X2}{g:X2}{b:X2}";

        private svg Chart()
        {
            chartstart = new DateTime(Chartdate.Year, Chartdate.Month, 1);
            daysinmonth = DateTime.DaysInMonth(chartstart.Year, chartstart.Month);
            daysdiff = Convert.ToInt32((chartstart.Date - Birthdate.Date).TotalDays) - 1;

            width = daysinmonth * Daywidth;
            center = (Height / 2) + Daywidth;
            amp = Convert.ToInt32((Height / 2) - (Daywidth * 1.5));

            svg = new svg
            {
                width = width,
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
            svg.Children.Add(defs1);

            DrawBackground();

            var ppts = DrawCycle(RedColor, 23);
            var epts = DrawCycle(GreenColor, 28);
            var ipts = DrawCycle(BlueColor, 33);
            LabelCycles(ppts, epts, ipts);

            return svg;
        }

        private void DrawBackground()
        {
            // draw the day grid
            for (var d = 0; d < daysinmonth; d++)
            {
                svg.Children.Add(new rect { width = Daywidth, height = Height, x = d * Daywidth, y = 0, fill = "url(#grad1)", stroke_width = 0 });
            }

            // draw a month divider hint line
            svg.Children.Add(new rect { width = 1, height = Height, fill = DarkBlueColor, x = 0, y = 0, stroke_width = 0 });

            // color the chart date only in the right month
            if (chartstart.Year == Chartdate.Year
                && chartstart.Month == Chartdate.Month
                && chartstart.Year == DateTime.Today.Year
                && chartstart.Month == DateTime.Today.Month)
            {
                svg.Children.Add(new rect { width = Daywidth, height = Height, fill = LighBlueColor, x = Daywidth * (Chartdate.Day - 1), y = 0 });
            }

            // draw the day numbers
            var day = chartstart;
            for (var d = 0; d < daysinmonth; d++)
            {
                svg.Children.Add(new text
                {
                    content = $"{day:ddd}",
                    x = (d * Daywidth) + (Daywidth / 2),
                    y = Daywidth / 2,
                    fill = DarkBlueColor,
                    text_anchor = "middle",
                    dominant_baseline = "middle",
                    font_family = FontFamily,
                    font_size = Daywidth / 2,
                    font_weight = "normal"
                });
                svg.Children.Add(new text
                {
                    content = $"{day:MMM}",
                    x = (d * Daywidth) + (Daywidth / 2),
                    y = Daywidth,
                    fill = DarkBlueColor,
                    text_anchor = "middle",
                    dominant_baseline = "middle",
                    font_family = FontFamily,
                    font_size = Daywidth / 2,
                    font_weight = "normal"
                });
                svg.Children.Add(new text
                {
                    content = $"{day.Day}",
                    x = (d * Daywidth) + (Daywidth / 2),
                    y = Daywidth * 1.5,
                    fill = DarkBlueColor,
                    text_anchor = "middle",
                    dominant_baseline = "middle",
                    font_family = FontFamily,
                    font_size = Daywidth / 2,
                    font_weight = "normal"
                });
                day = day.AddDays(1);
            }

            // draw the critical line (note: can't use dropshadow filter on 'line' element
            svg.Children.Add(new rect
            {
                x = 0,
                y = center - (Daywidth / 8),
                width = width,
                height = Daywidth / 4,
                fill = GoldColor,
                stroke_width = 0,
                filter = "url(#dropShadow)"
            });
        }

        private IList<Point> DrawCycle(string color, int cycle)
        {
            var diff = daysdiff - .5d; // the .5 offsets the sin wave half a day so it looks nicer
            var coords = new List<Point>();
            for (var j = -1; j <= (daysinmonth + 1) * 2; j++, diff += .5d)
            {
                coords.Add(new Point { X = ((j - 1) * Daywidth) / 2, Y = Convert.ToInt32(center - (amp * Math.Sin((diff / cycle) * Math.PI * 2))) });
            }

            // draw a smooth curve
            var sb = new StringBuilder($"M {coords[1].X} {coords[1].Y}");
            foreach (var pt in coords.Skip(2))
            {
                sb.Append($" L {pt.X} {pt.Y}");
            }

            svg.Children.Add(new path
            {
                d = sb.ToString(),
                stroke = color,
                stroke_width = Daywidth / 3,
                fill = Transparent,
                filter = "url(#dropShadow)"
            });

            return coords;
        }

        private void LabelCycles(IList<Point> pp, IList<Point> ep, IList<Point> ip)
        {
            // locate the best place to draw the cycle labels
            var clx = 0;
            var plx = 0;
            var elx = 0;
            var ilx = 0;
            var ply = 0;
            var ely = 0;
            var ily = 0;
            var minyoffset = Convert.ToInt32(Daywidth * 1.25);
            const int minday = 3;
            var maxday = pp.Count - 6;
            var criticals = new List<int>();

            for (var d = 1; d < pp.Count; ++d)
            {
                var p0 = pp[d - 1].Y;
                var e0 = ep[d - 1].Y;
                var i0 = ip[d - 1].Y;
                var p1 = pp[d].Y;
                var e1 = ep[d].Y;
                var i1 = ip[d].Y;

                if (d >= minday && d <= maxday && plx == 0
                    && Math.Abs(p1 - e1) > minyoffset
                    && Math.Abs(p1 - i1) > minyoffset
                    && Math.Abs(p1 - center) > minyoffset
                    && Math.Abs(p1 - ely) > minyoffset
                    && Math.Abs(p1 - ily) > minyoffset)
                {
                    plx = pp[d].X;
                    ply = p1;
                }

                if (d >= minday && d <= maxday && elx == 0
                    && Math.Abs(e1 - p1) > minyoffset
                    && Math.Abs(e1 - i1) > minyoffset
                    && Math.Abs(e1 - center) > minyoffset
                    && Math.Abs(e1 - ply) > minyoffset
                    && Math.Abs(e1 - ily) > minyoffset)
                {
                    elx = ep[d].X;
                    ely = e1;
                }

                if (d >= minday && d <= maxday && ilx == 0
                    && Math.Abs(i1 - p1) > minyoffset
                    && Math.Abs(i1 - e1) > minyoffset
                    && Math.Abs(i1 - center) > minyoffset
                    && Math.Abs(i1 - ply) > minyoffset
                    && Math.Abs(i1 - ely) > minyoffset)
                {
                    ilx = ip[d].X;
                    ily = i1;
                }

                if (d >= minday && d <= maxday && clx == 0
                    && Math.Abs(p1 - center) > minyoffset * 2
                    && Math.Abs(e1 - center) > minyoffset * 2
                    && Math.Abs(i1 - center) > minyoffset * 2)
                {
                    clx = ip[d].X;  // use x of physical rhythm here
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

            if (plx == 0)
            {
                plx = pp[1].X;
                ply = pp[1].Y;
            }

            if (elx == 0)
            {
                elx = ep[1].X;
                ely = ep[1].Y;
            }

            if (ilx == 0)
            {
                ilx = ip[1].X;
                ily = ip[1].Y;
            }

            if (clx == 0)
            {
                clx = 2 * Daywidth;
            }

            // draw the critical circles
            foreach (var cc in criticals.Distinct())
            {
                svg.Children.Add(new circle
                {
                    cx = cc,
                    cy = center,
                    r = Daywidth / 3,
                    fill = Transparent,
                    stroke = GoldColor,
                    stroke_width = 5
                    // filter = "url(#dropShadow)" // squares off corners too!
                });
            }

            // Labels
            ShadowText("critical", clx, center, GoldColor);
            ShadowText("physical", plx + Daywidth, ply, RedColor);
            ShadowText("emotional", elx + Daywidth, ely, GreenColor);
            ShadowText("intellectual", ilx + Daywidth, ily, BlueColor);
        }

        private void ShadowText(string text, double x, double y, string color)
        {
            svg.Children.Add(new text
            {
                x = x,
                y = y,
                dominant_baseline = "middle",
                // TextAlignment = TextAlignment.Left,
                font_family = FontFamily,
                font_size = Daywidth,
                content = text,
                // VerticalAlignment = VerticalAlignment.Top,
                // Padding = new Thickness(0),
                fill = color,
                filter = "url(#dropShadow)"
            });
        }

        private class Point
        {
            public int X { get; init; }

            public int Y { get; init; }
        }
    }
}