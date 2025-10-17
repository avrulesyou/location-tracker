using LocationHeatmap.Models;
using Microsoft.Maui.Controls.Maps;
using System.Collections.Generic;

namespace LocationHeatmap.Controls
{
    public class HeatmapLayer : GraphicsView, IMapLayer
    {
        private readonly HeatmapDrawable _drawable = new();

        public List<LocationPoint> Points
        {
            get => _drawable.Points;
            set => _drawable.Points = value;
        }

        public HeatmapLayer(Map map)
        {
            _drawable.Map = map;
            this.Drawable = _drawable;
            this.InputTransparent = true;
        }

        public void Update() => Invalidate();
    }
}
