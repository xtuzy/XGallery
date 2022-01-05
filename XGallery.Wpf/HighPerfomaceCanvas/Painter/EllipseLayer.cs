using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CanvasDemo.Canvas;
using SkiaSharp;

namespace CanvasDemo.Painter
{
    public class EllipseLayer : Layer
    {
        public EllipseLayer(TimCanvas canvas) : base(canvas, "Cube")
        {
            IsInteractionLayer = true;
        }

        public override void Drawing(SKCanvas g)
        {
            foreach (var item in Elements)
            {
                if (Canvas.Viewer.IsInZone(item) == false) continue;
                item.Drawing(g);
            }
        }

        public override void DrawingAfter(SKCanvas g)
        {
     
        }
    }
}
