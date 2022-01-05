using SkiaSharp;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Point = SkiaSharp.SKPoint;
using Rectangle = SkiaSharp.SKRect;
using Size = SkiaSharp.SKSize;
namespace CanvasDemo.Canvas
{
    public class SelectionBox : Element
    {
        /// <summary>
        /// 是否从左往右选择
        /// </summary>
        public bool IsLeftToRight { get; set; } = true;

        public bool SelectionBoxIsShow { get; set; } = false;

        //框选开始坐标
        Point Start;
        //鼠标中键按下
        bool IsMouseLeftDown = false;

        ElementEditor Editor;

        public SelectionBox(ElementEditor editor, TimCanvas canvas) : base(canvas,nameof(SelectionBox))
        {
            Editor = editor;
        }

        public override void Drawing(SKCanvas g)
        {
            if (SelectionBoxIsShow == true)
            {
                using(var paint = new SKPaint())
                {
                    if (IsLeftToRight == true)
                    {
                        paint.Color = SKColor.Parse($"{51:X2}{153:X2}{255:X2}").WithAlpha(100);
                        paint.Style = SKPaintStyle.Fill;
                        g.DrawRect(Viewer.LocalToShow(Rect),paint);
                        paint.Color = SKColor.Parse($"{51:X2}{153:X2}{255:X2}").WithAlpha(255);
                        paint.Style = SKPaintStyle.Stroke;
                        g.DrawRect(Viewer.LocalToShow(Rect),paint);
                    }
                    else
                    {
                        paint.Color = SKColor.Parse($"{153:X2}{255:X2}{51:X2}").WithAlpha(100);
                        paint.Style = SKPaintStyle.Fill;
                        g.DrawRect(Viewer.LocalToShow(Rect), paint);
                        paint.Color = SKColor.Parse($"{153:X2}{255:X2}{51:X2}").WithAlpha(255);
                        paint.Style = SKPaintStyle.Stroke;
                        g.DrawRect(Viewer.LocalToShow(Rect), paint);
                    }
                }
            }
        }

        public void MouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton==MouseButtonState.Pressed)
            {
                IsMouseLeftDown = true;
                Start = Viewer.MousePointToLocal(e.GetPosition(Canvas).ToSKPoint());
                Rect.Size = new Size(0,0);
                SelectionBoxIsShow = true;
            }
        }

        public void MouseMove(MouseEventArgs e)
        {
            //比例缩放后结束坐标也要做调整
            if (IsMouseLeftDown == true)
            {
                var end = Viewer.MousePointToLocal(e.GetPosition(Canvas).ToSKPoint());

                IsLeftToRight = Start.X < end.X;
                Rect.Location = new Point(Start.X < end.X ? Start.X : end.X, Start.Y < end.Y ? Start.Y : end.Y);
                Rect.Size = new Size(Math.Abs(Start.X - end.X), Math.Abs(Start.Y - end.Y));
            }
        }

        public void MouseUp(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsMouseLeftDown = false;
                SelectionBoxIsShow = false;

                var end = Viewer.MousePointToLocal(e.GetPosition(Canvas).ToSKPoint());
                if (end.Distance(Start) < 15)
                {//开始和结束距离短，认为是鼠标点击选择
                    PointSelectOver(e.GetPosition(Canvas).ToSKPoint());
                }
                else
                {
                    BoxSelectOver();
                }

            }
        }

        public void MouseWheel(MouseWheelEventArgs e)
        {

        }

        /// <summary>
        /// 选择单个对象
        /// </summary>
        private void PointSelectOver(Point mousePoint)
        {
            //if (Control.ModifierKeys != Keys.Control)
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)//https://stackoverflow.com/a/5750848/13254773
            {
                Editor.ClearSelected();
            }
            var point = Viewer.MousePointToLocal(mousePoint);
            foreach (var item in Canvas.Layers)
            {
                if (item.IsActive == false) continue;
                var elm = item.Elements.FirstOrDefault(x => x.Rect.Contains(point) == true);
                if (elm != null)
                {

                    Editor.AddSelected(new List<ObjElement>() { elm });
                    Editor.SetCurrent(elm);
                    return;
                }
            }

        }

        /// <summary>
        /// 选择被框选的对象
        /// </summary>
        private void BoxSelectOver()
        {
            //if (Control.ModifierKeys != Keys.Control)
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)//https://stackoverflow.com/a/5750848/13254773
            {
                //撤销以前的选择
                Editor.ClearSelected();
            }
            foreach (var item in Canvas.Layers)
            {
                if (item.IsActive == false) continue;

                if (IsLeftToRight == true)
                {//全部选中才算选中
                    Editor.AddSelected(item.Elements.AsParallel().Where(x => Rect.Contains(x.Rect) == true).ToList());
                }
                else
                {//相交就认为已经选中
                    Editor.AddSelected(item.Elements.AsParallel().Where(x => x.Rect.IntersectsWith(Rect) == true).ToList());
                }
            }
            Editor.SetCurrent(Editor.SelectedElements.FirstOrDefault());
        }

        public override void DrawingAfter(SKCanvas g)
        {
        
        }


    }
}
