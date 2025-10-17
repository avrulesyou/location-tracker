using LocationHeatmap.Models;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace LocationHeatmap.Controls
{
    public class HeatmapDrawable : IDrawable
    {
        public List<LocationPoint> Points { get; set; } = new();
        public Map Map { get; set; }
        public double Radius { get; set; } = 80;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Map?.VisibleRegion == null) return;

            var paint = new Paint
            {
                Shader = new RadialGradientPaint(
                    new PaintGradientStop[]
                    {
                        new(0, Color.FromRgba(255, 0, 0, 150)),
                        new(1, Colors.Transparent)
                    },
                    new Point(0, 0), Radius / 2)
            };

            foreach (var point in Points)
            {
                Point screenPoint = GetScreenCoordinates(point.Latitude, point.Longitude, dirtyRect.Size);
                if (dirtyRect.Contains(screenPoint))
                {
                    canvas.SetFillPaint(paint, RectF.Create(screenPoint, new Size(1, 1)));
                    canvas.FillCircle((float)screenPoint.X, (float)screenPoint.Y, (float)Radius);
                }
            }
        }

        private Point GetScreenCoordinates(double latitude, double longitude, Size viewSize)
        {
            double mapLat = Map.VisibleRegion.Center.Latitude;
            double mapLon = Map.VisibleRegion.Center.Longitude;
            double mapLatDelta = Map.VisibleRegion.LatitudeDegrees;
            double mapLonDelta = Map.VisibleRegion.LongitudeDegrees;

            double x = (longitude - (mapLon - mapLonDelta / 2)) * (viewSize.Width / mapLonDelta);
            double y = ((mapLat + mapLatDelta / 2) - latitude) * (viewSize.Height / mapLatDelta);

            return new Point(x, y);
        }
    }
}
