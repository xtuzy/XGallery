using CanvasDemo.Canvas;
using CanvasDemo.Data;
using SkiaSharp;
using System;
using System.Collections.Generic;

using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDemo.Painter
{
    public class EllipseElement : ObjElement<ElementData>
    {
        public EllipseElement(EllipseLayer layer, ElementData data, int sideLength) : base(layer, data)
        {
            this.Rect.Size =new SKSize( sideLength,sideLength);
        }

        public static SKPaint FillBrush = new SKPaint() {Color= SKColors.Green };

        public override void Drawing(SKCanvas g)
        {
            g.DrawOval( Viewer.LocalToShow(Rect), FillBrush);
        }

        public override void DrawingAfter(SKCanvas g)
        {

        }

    }

}
