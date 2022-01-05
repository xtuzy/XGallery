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
    /// 绘制十字焦点
    /// </summary>
    public class FocusElement : Element
    {
        public bool IsShow { get; set; } = false;
        public Point Focus { get; private set; }

        //public Point H1, H2;

        //public Point V1, V2;

        public void SetFocus(Point focus)
        {
            Focus = focus;
            //H1 = new Point(focus.X, 0);
            //H2 = new Point(focus.X, this.Canvas.Height);

            //V1 = new Point(0, focus.Y);
            //V2 = new Point(this.Canvas.Width, focus.Y);

        }

        public FocusElement(Canvas.TimCanvas canvas) : base(canvas,nameof(FocusElement))
        {
        }

        public override void Drawing(SKCanvas g)
        {
            var focus = this.Canvas.Viewer.LocalToShow(Focus);
            using(var paint = new SKPaint() { Color = SKColors.Black })
            {
                g.DrawLine(new Point(focus.X, 0), new Point(focus.X, (float)this.Canvas.Height),paint);
                g.DrawLine( new Point(0, focus.Y), new Point((float)this.Canvas.Width, focus.Y),paint);
            }
            
        }

        public override void DrawingAfter(SKCanvas g)
        {
          
        }
    }
}
