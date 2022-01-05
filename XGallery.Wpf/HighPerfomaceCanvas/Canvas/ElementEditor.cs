using SkiaSharp;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Point = SkiaSharp.SKPoint;
using Rectangle = SkiaSharp.SKRect;
using Size = SkiaSharp.SKSize;
namespace CanvasDemo.Canvas
{
    /// <summary>
    /// 选择框对象
    /// </summary>
    public class ElementEditor : Element
    {
        SelectionBox SelectionBox;

        public ElementEditor(TimCanvas canvas) : base(canvas, nameof(ElementEditor))
        {
            SelectionBox = new SelectionBox(this, canvas);

        }

        public override void Drawing(SKCanvas g)
        {
            //绘制选择对象的拖动柄
            SelectedElements.ForEach(x => x.DrawingJoystick(g));

            SelectionBox.Drawing(g);
        }

        public Point MPoint;
        //对象状态
        private EditorState EState = EditorState.None;
        //拖动柄状态
        private TransformState TState = TransformState.None;

        public void MouseDown(MouseButtonEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //MPoint = Viewer.MousePointToLocal(e.Location);
                MPoint = Viewer.MousePointToLocal(e.GetPosition(e.Source as FrameworkElement).ToSKPoint());
                var elem = SelectedElements.FirstOrDefault(x => x.Rect.Contains(MPoint) == true);
                if (elem != null)
                {//点击已经选择的对象
                    SetCurrent(elem);//设定当前点的对象为操作对象

                    if (Canvas.IsLocked == false)
                    {//如果是只读，那么就不要进入移动和调整大小模式
                        if (MPoint.X > elem.Rect.Right - elem.JoystickSize && MPoint.Y > elem.Rect.Bottom - elem.JoystickSize && MPoint.X < elem.Rect.Right && MPoint.Y < elem.Rect.Bottom)
                        {
                            EState = EditorState.Transform;
                            TState = TransformState.RightBottom;
                        }
                        else if (MPoint.X > elem.Rect.Right - elem.JoystickSize && MPoint.Y > elem.Rect.Top + elem.Rect.Height / 2 - elem.JoystickSize / 2 && MPoint.X < elem.Rect.Right && MPoint.Y < elem.Rect.Top + elem.Rect.Height / 2 + elem.JoystickSize / 2)
                        {
                            EState = EditorState.Transform;
                            TState = TransformState.Right;
                        }
                        else if (MPoint.X > elem.Rect.Left + elem.Rect.Width / 2 - elem.JoystickSize / 2 && MPoint.Y > elem.Rect.Bottom - elem.JoystickSize && MPoint.X < elem.Rect.Left + elem.Rect.Width / 2 + elem.JoystickSize / 2 && MPoint.Y < elem.Rect.Bottom)
                        {
                            EState = EditorState.Transform;
                            TState = TransformState.Bottom;
                        }
                        else
                        {
                            EState = EditorState.Move;
                        }
                    }

                }
                else
                {
                    EState = EditorState.Selection;
                    SelectionBox.MouseDown(e);
                }
            }
        }

        public void MouseMove(MouseEventArgs e)
        {
            if (EState == EditorState.Move)
            {//移动模式，设定对象位置
                var end = Viewer.MousePointToLocal(e.GetPosition(e.Source as FrameworkElement).ToSKPoint());
                var x = (end.X - MPoint.X);
                var y = (end.Y - MPoint.Y);

                SelectedElements.ForEach(elem => elem.Rect.Offset(x, y));
                MPoint = end;
            }
            else if (EState == EditorState.Transform)
            {//调整大小
                var end = Viewer.MousePointToLocal(e.GetPosition(e.Source as FrameworkElement).ToSKPoint());
                var x = (end.X - MPoint.X);
                var y = (end.Y - MPoint.Y);

                SelectedElements.ForEach(elem =>
                {
                    switch (TState)
                    {
                        case TransformState.RightBottom:
                            elem.Rect.Size = new Size(elem.Rect.Width + x, elem.Rect.Height + y);
                            break;
                        case TransformState.Right:
                            elem.Rect.Size = new Size(elem.Rect.Width + x, elem.Rect.Height);
                            break;
                        case TransformState.Bottom:
                            elem.Rect.Size = new Size(elem.Rect.Width,elem.Rect.Height + y);
                            break;
                    }

                    if (elem.Rect.Width < 10) elem.Rect.Size = new Size(10, elem.Rect.Height);
                    if (elem.Rect.Height < 10) elem.Rect.Size = new Size(elem.Rect.Width, 10);
                });

                MPoint = end;
            }
            else if (EState == EditorState.Selection)
            {//选择
                SelectionBox.MouseMove(e);

            }
        }

        public void MouseUp(MouseButtonEventArgs e)
        {
            if (EState == EditorState.Selection)
            {
                SelectionBox.MouseUp(e);
            }

            EState = EditorState.None;
        }

        public void MouseWheel(MouseWheelEventArgs e)
        {

        }

        #region 对象选择操作

        /// <summary>
        /// 选择的对象集合
        /// </summary>
        public List<ObjElement> SelectedElements = new List<ObjElement>();

        /// <summary>
        /// 当前对象
        /// </summary>
        public ObjElement CurrentElement = null;

        /// <summary>
        /// 清除选的的对象
        /// </summary>
        public void ClearSelected()
        {
            SelectedElements.ForEach(x => x?.UnSelected());
            SelectedElements.Clear();
        }

        /// <summary>
        /// 设置当前选择的对象
        /// </summary>
        /// <param name="element"></param>
        public void SetCurrent(ObjElement element)
        {
            CurrentElement?.UnCurrent();
            CurrentElement = element;
            CurrentElement?.Current();
        }

        /// <summary>
        /// 添加选中的对象
        /// </summary>
        /// <param name="elements"></param>
        public void AddSelected(List<ObjElement> elements)
        {
            SelectedElements.AddRange(elements);
            elements.ForEach(x => x?.Selected());

            SelectedObjElementsEvent?.Invoke(SelectedElements);

        }
        /// <summary>
        /// 选择了对象元素后触发此方法
        /// </summary>
        public event Action<List<ObjElement>> SelectedObjElementsEvent;



        #endregion



        #region 元素操作

        /// <summary>
        /// 被删除的对象,可以理解成当前布局的回收站，可以从这里拿回已经删除的对象
        /// </summary>
        public Dictionary<string, ObjElement> DeletedElems = new Dictionary<string, ObjElement>();

        /// <summary>
        /// 移除选择的对象
        /// </summary>
        public void RemoveSelectElements()
        {
            foreach (var item in SelectedElements)
            {
                item.Layer.Elements.Remove(item);
                DeletedElems.Add(item.ID, item);
            }
            ClearSelected();

            SelectedObjElementsEvent?.Invoke(SelectedElements);

        }

        #endregion

        #region 对象布局操作

        /// <summary>
        /// 左对齐
        /// </summary>
        public void AlignLeft()
        {
            if (SelectedElements.Count <= 1) return;

            foreach (var item in SelectedElements)
            {
                item.Rect.Left = CurrentElement.Rect.Left;
            }
            Canvas.Refresh();
        }

        /// <summary>
        /// 右对齐
        /// </summary>
        public void AlignRight()
        {
            if (SelectedElements.Count <= 1) return;

            foreach (var item in SelectedElements)
            {
                item.Rect.Left = CurrentElement.Rect.Right - item.Rect.Width;
            };
            Canvas.Refresh();
        }

        /// <summary>
        /// 上对齐
        /// </summary>
        public void AlignTop()
        {
            if (SelectedElements.Count <= 1) return;

            foreach (var item in SelectedElements)
            {
                item.Rect.Top = CurrentElement.Rect.Top;
            };
            Canvas.Refresh();
        }

        /// <summary>
        /// 下对齐
        /// </summary>
        public void AlignBottom()
        {
            if (SelectedElements.Count <= 1) return;

            foreach (var item in SelectedElements)
            {
                item.Rect.Top = CurrentElement.Rect.Bottom - item.Rect.Height;
            };
            Canvas.Refresh();
        }

        /// <summary>
        /// 居中齐
        /// </summary>
        public void AlignCenter()
        {
            if (SelectedElements.Count <= 1) return;

            var center = CurrentElement.Rect.Left + CurrentElement.Rect.Width / 2;

            foreach (var item in SelectedElements)
            {
                item.Rect.Left = center - item.Rect.Width / 2;
            };
            Canvas.Refresh();
        }

        /// <summary>
        /// 中间齐
        /// </summary>
        public void AlignMiddle()
        {
            if (SelectedElements.Count <= 1) return;

            var middle = CurrentElement.Rect.Top + CurrentElement.Rect.Height / 2;

            foreach (var item in SelectedElements)
            {
                item.Rect.Top = middle - item.Rect.Height / 2;
            };
            Canvas.Refresh();
        }

        /// <summary>
        /// 宽度相同
        /// </summary>
        public void SameWidth()
        {
            if (SelectedElements.Count <= 1) return;

            foreach (var item in SelectedElements)
            {
                item.Rect.Size= new Size( CurrentElement.Rect.Width,item.Rect.Height);
            };
            Canvas.Refresh();
        }

        /// <summary>
        /// 高度相同
        /// </summary>
        public void SameHeight()
        {
            if (SelectedElements.Count <= 1) return;

            foreach (var item in SelectedElements)
            {
                item.Rect.Size = new Size(item.Rect.Width, CurrentElement.Rect.Height);
            };
            Canvas.Refresh();
        }

        /// <summary>
        /// 大小相同
        /// </summary>
        public void SameSize()
        {
            if (SelectedElements.Count <= 1) return;

            foreach (var item in SelectedElements)
            {
                item.Rect.Size = new Size(CurrentElement.Rect.Width, CurrentElement.Rect.Height);
            };
            Canvas.Refresh();
        }

        /// <summary>
        /// 水平间距相同
        /// </summary>
        public void SameHorizontalSpace()
        {
            if (SelectedElements.Count <= 1) return;

            var orderFans = SelectedElements.OrderBy(x => x.Rect.Left).ToList();
            var minLeft = orderFans.First().Rect.Left;
            var maxLeft = orderFans.Last().Rect.Left;
            var distance = maxLeft - minLeft;

            for (int i = 0; i < orderFans.Count; i++)
            {
                orderFans[i].Rect.Left = (int)(minLeft + (float)i / ((float)orderFans.Count - 1.0f) * distance);
            }
            Canvas.Refresh();
        }

        /// <summary>
        /// 垂直间距相同
        /// </summary>
        public void SameVerticalSpace()
        {
            if (SelectedElements.Count <= 1) return;

            var orderFans = SelectedElements.OrderBy(x => x.Rect.Top).ToList();
            var minTop = orderFans.First().Rect.Top;
            var maxTop = orderFans.Last().Rect.Top;
            var distance = maxTop - minTop;

            for (int i = 0; i < orderFans.Count; i++)
            {
                orderFans[i].Rect.Top = (int)(minTop + (float)i / ((float)orderFans.Count - 1.0f) * distance);
            }
            Canvas.Refresh();
        }

        public override void DrawingAfter(SKCanvas g)
        {

        }


        #endregion


        enum EditorState
        {
            None,//没有任何操作
            Selection,//框选状态
            Move,//移动状态
            Transform,//调整大小状态
        }


        enum TransformState
        {
            None,
            RightBottom,
            Right,
            Bottom,

        }
    }



}