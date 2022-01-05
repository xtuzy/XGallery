using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = SkiaSharp.SKPoint;
using Rectangle = SkiaSharp.SKRect;
using Size = SkiaSharp.SKSize;
namespace CanvasDemo.Canvas
{

    /// <summary>
    /// 控制背景
    /// </summary>
    public class Backgrounder 
    {
        TimCanvas Canvas;

        public Backgrounder(TimCanvas canvas)
        {
            Canvas = canvas;
        }

        //Pen ZeroLinePen = new Pen(new SolidBrush(Color.Black),2);
        SKPaint ZeroLinePen = new SKPaint() { Color=SKColors.Black,StrokeWidth=2};

        public void Drawing(SKCanvas g)
        {
            var v = Canvas.Viewer.Viewport;

            var vP1 = new Point(0 , v.Top);
            var vP2 = new Point(0 , v.Top + v.Height);
            g.DrawLine( Canvas.Viewer.LocalToShow(vP1), Canvas.Viewer.LocalToShow(vP2),ZeroLinePen);

            var hP1 = new Point(v.Left, 0);
            var hP2 = new Point(v.Left+v.Width, 0);
            g.DrawLine(Canvas.Viewer.LocalToShow(hP1), Canvas.Viewer.LocalToShow(hP2), ZeroLinePen);
        }

    }


}
