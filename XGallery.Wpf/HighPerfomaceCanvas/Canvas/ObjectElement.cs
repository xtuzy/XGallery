using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Helper.Views;
using Point = SkiaSharp.SKPoint;
using Rectangle = SkiaSharp.SKRect;
using Size = SkiaSharp.SKSize;
namespace CanvasDemo.Canvas
{
    /// <summary>
    /// 对象元素
    /// </summary>
    public abstract class ObjElement<T> : ObjElement where T : IElementData
    {
        /// <summary>
        /// 对象实体
        /// </summary>
        public T Data { get; set; }

        public ObjElement(Layer layer, T data) : base(layer, data.ID)
        {
            Data = data;
        }
    }

    public abstract class ObjElement : Element
    {

        public Layer Layer;

        public ObjElement(Layer layer, string id) : base(layer.Canvas, id)
        {
            Layer = layer;
            Layer.Elements.Add(this);
        }

        /// <summary>
        /// 是否非选中
        /// </summary>
        public bool IsSelected { get; private set; } = false;

        /// <summary>
        /// 是否是当前对象
        /// </summary>
        public bool IsCurrent { get; private set; } = false;

        /// <summary>
        /// 选择对象
        /// </summary>
        public void Selected()
        {
            IsSelected = true;
            SelectedEvent();
        }
        protected virtual void SelectedEvent() { }

        /// <summary>
        /// 清除对象选择
        /// </summary>
        public void UnSelected()
        {
            IsSelected = false;
            UnSelectedEvent();
        }
        protected virtual void UnSelectedEvent() { }

        public void Current()
        {
            IsCurrent = true;
            CurrentEvent();
        }
        protected virtual void CurrentEvent() { }

        public void UnCurrent()
        {
            IsCurrent = false;
            UnCurrentEvent();
        }
        protected virtual void UnCurrentEvent() { }


        public int JoystickSize { get { return (int)((Rect.Width + Rect.Height) / 20 + 1); } }

        static SKPaint JoystickCurrent = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = SKColor.Parse($"{255:X2}{255:X2}{255:X2}").WithAlpha(230),
        };
        static SKPaint JoystickSelect = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = SKColor.Parse($"{50:X2}{50:X2}{50:X2}").WithAlpha(230),
        };
        /// <summary>
        /// 绘制八个操纵柄
        /// </summary>
        /// <param name="painter"></param>
        public void DrawingJoystick(SKCanvas g)
        {
            if (IsSelected == false) return;

            var cX = Rect.Width / 2;
            var cY = Rect.Height / 2;
            var s = JoystickSize;
            if (IsCurrent == false)
            {
                g.DrawRect( Viewer.LocalToShow(Rect.Left, Rect.Top + (0), s, s), JoystickSelect);
                g.DrawRect( Viewer.LocalToShow(Rect.Left + (cX - s / 2), Rect.Top + (0), s, s),JoystickSelect);
                g.DrawRect(Viewer.LocalToShow(Rect.Left + (Rect.Width - s), Rect.Top + (0), s, s),JoystickSelect);
                g.DrawRect( Viewer.LocalToShow(Rect.Left + (Rect.Width - s), Rect.Top + (cY - s / 2), s, s),JoystickSelect);
                g.DrawRect(Viewer.LocalToShow(Rect.Left + (Rect.Width - s), Rect.Top + (Rect.Height - s), s, s),JoystickSelect);
                g.DrawRect( Viewer.LocalToShow(Rect.Left + (cX - s / 2), Rect.Top + (Rect.Height - s), s, s),JoystickSelect);
                g.DrawRect(Viewer.LocalToShow(Rect.Left + (0), Rect.Top + (Rect.Height - s), s, s),JoystickSelect);
                g.DrawRect( Viewer.LocalToShow(Rect.Left + (0), Rect.Top + (cY - s / 2), s, s),JoystickSelect);
            }
            else if (IsCurrent == true)
            {
                g.DrawRect(Viewer.LocalToShow(Rect.Left, Rect.Top + (0), s, s),JoystickCurrent);
                g.DrawRect(Viewer.LocalToShow(Rect.Left + (cX - s / 2), Rect.Top + (0), s, s),JoystickCurrent);
                g.DrawRect(Viewer.LocalToShow(Rect.Left + (Rect.Width - s), Rect.Top + (0), s, s),JoystickCurrent);
                g.DrawRect(Viewer.LocalToShow(Rect.Left + (Rect.Width - s), Rect.Top + (cY - s / 2), s, s),JoystickCurrent);
                g.DrawRect(Viewer.LocalToShow(Rect.Left + (Rect.Width - s), Rect.Top + (Rect.Height - s), s, s),JoystickCurrent);
                g.DrawRect(Viewer.LocalToShow(Rect.Left + (cX - s / 2), Rect.Top + (Rect.Height - s), s, s),JoystickCurrent);
                g.DrawRect(Viewer.LocalToShow(Rect.Left + (0), Rect.Top + (Rect.Height - s), s, s),JoystickCurrent);
                g.DrawRect(Viewer.LocalToShow(Rect.Left + (0), Rect.Top + (cY - s / 2), s, s), JoystickCurrent);
                using (var paint = new SKPaint() { Color = SKColors.Black })
                {
                    g.DrawRect(Viewer.LocalToShow(Rect.Left, Rect.Top + (0), s, s), paint);
                    g.DrawRect(Viewer.LocalToShow(Rect.Left + (cX - s / 2), Rect.Top + (0), s, s), paint);
                    g.DrawRect(Viewer.LocalToShow(Rect.Left + (Rect.Width - s), Rect.Top + (0), s, s),paint);
                    g.DrawRect(Viewer.LocalToShow(Rect.Left + (Rect.Width - s), Rect.Top + (cY - s / 2), s, s),paint);
                    g.DrawRect(Viewer.LocalToShow(Rect.Left + (Rect.Width - s), Rect.Top + (Rect.Height - s), s, s),paint);
                    g.DrawRect(Viewer.LocalToShow(Rect.Left + (cX - s / 2), Rect.Top + (Rect.Height - s), s, s),paint);
                    g.DrawRect(Viewer.LocalToShow(Rect.Left + (0), Rect.Top + (Rect.Height - s), s, s),paint);
                    g.DrawRect(Viewer.LocalToShow(Rect.Left + (0), Rect.Top + (cY - s / 2), s, s),paint);
                }
            }
        }



    }


}
