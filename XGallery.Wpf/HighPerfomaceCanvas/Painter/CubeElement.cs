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
    public class CubeElement : ObjElement<ElementData>, IToolTipElement
    {


        public CubeElement(CubeLayer layer, ElementData data, int sideLength) : base(layer, data)
        {
            this.Rect.Size = new SKSize(sideLength, sideLength);
        }

        public bool IsHover { get; set; } = false;

        public static SKPaint FillBrush = new SKPaint() { Color = SKColors.Blue };

        public static SKPaint ErrorBrush = new SKPaint() { Color = SKColors.Red };



        public override void Drawing(SKCanvas g)
        {

            var titleH = (int)(Rect.Height * 0.25);

            var fillBrush = Data.IsError ? ErrorBrush : FillBrush;

            if (titleH * Viewer.Zoom > 10)//如果标题大于10就认真绘制，如哦小于那么就简化
            {
                var borderW = (int)(Rect.Height * 0.01 * Viewer.Zoom) + 1;
                using (var paint = new SKPaint() { Color = SKColors.White })
                {
                    g.DrawRect(Viewer.LocalToShow(Rect.Left, Rect.Top, Rect.Width, titleH + 1), paint);
                    var fontSize = (int)(titleH / 2 * Viewer.Zoom) + 1;
                    if (fontSize >= 3)
                    {
                        paint.Typeface = SKTypeface.FromFamilyName("微软雅黑");
                        paint.TextSize = fontSize > 60 ? 60 : fontSize;
                        paint.Color = SKColors.Black;
                        //g.DrawText(Data.Title.ToString(),Viewer.LocalToShow(Rect.Left + (int)(borderW / Viewer.Zoom), Rect.Top + (int)(borderW / Viewer.Zoom), Rect.Width, Rect.Height));
                        g.DrawText(Data.Title.ToString(), Viewer.LocalToShow(Rect.Left + (int)(borderW / Viewer.Zoom), Rect.Top + (int)(borderW / Viewer.Zoom), Rect.Width, Rect.Height).Location, paint);
                    }

                    var contentRect = Viewer.LocalToShow(Rect.Left, Rect.Top + titleH, Rect.Width, Rect.Height - titleH);
                    g.DrawRect(contentRect, fillBrush);
                    paint.Color = SKColors.White;
                    paint.TextSize = (Rect.Height - titleH) / 2 * Viewer.Zoom;
                    //g.DrawText(Data.Group.ToString(), contentRect, SFAlignment);
                    g.DrawText(Data.Group.ToString(), contentRect.Location, paint);


                    if (IsHover)
                    {
                        paint.Color = SKColors.Red;
                        paint.StrokeWidth = borderW * 2;
                        g.DrawRect(Viewer.LocalToShow(Rect), paint);
                    }

                    else
                    {
                        paint.Color = SKColors.Black;
                        paint.StrokeWidth = borderW;
                        g.DrawRect(Viewer.LocalToShow(Rect), paint);
                    }


                }

            }
            else if (titleH * Viewer.Zoom > 5)
            {
                g.DrawRect(Viewer.LocalToShow(Rect), fillBrush);

                var fontSize = (int)(titleH * Viewer.Zoom) + 1;
                if (fontSize >= 3)
                {
                    // g.DrawText(Data.Group.ToString(), Viewer.LocalToShow(Rect.Left + 1, Rect.Top + 1, Rect.Width, Rect.Height), SFAlignment);
                    g.DrawText(Data.Group.ToString(), Viewer.LocalToShow(Rect.Left + 1, Rect.Top + 1, Rect.Width, Rect.Height).Location,
                        new SKPaint()
                        {
                            Typeface = SKTypeface.FromFamilyName("微软雅黑"),
                            TextSize = fontSize > 60 ? 60 : fontSize,
                            Color = SKColors.White,
                        });
                }
            }
            else
            {
                g.DrawRect(Viewer.LocalToShow(Rect), fillBrush);
            }
        }

        public override void DrawingAfter(SKCanvas g)
        {

        }

        public string GetToolTipTitle()
        {
            return $"[{Data.Group}] {Data.Title}";
        }

       /* protected static StringFormat SFAlignment = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
        };*/
    }

}
