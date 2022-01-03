using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using SkiaSharp;
using System.Diagnostics;
using Xamarin.Helper.Logs;

#if __ANDROID__
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SkiaSharp.Views.Android;
#elif __IOS__
using SkiaSharp.Views.iOS;
using Foundation;
using UIKit;
#elif __UWP__
using SkiaSharp.Views.UWP;
#elif __WPF__
using SkiaSharp.Views.WPF;
using SkiaSharp.Views.Desktop;
#endif


namespace XGallery.Views
{
#if __ANDROID__
    public class LayerView : SKCanvasView
    {
#elif __IOS__
    public class LayerView : SKCanvasView
    {
#elif __UWP__
    public class LayerView : SKXamlCanvas
    {
#elif __WPF__
    public class LayerView : SkiaSharp.Views.WPF.SKElement
    {
#endif
        private Thread m_RenderThread = null;
        private AutoResetEvent m_ThreadGate = null;
        private List<Layer> m_Layers = null;
        private Layer m_Layer_Background = null;
        private Layer m_Layer_Grid = null;
        private Layer m_Layer_Data = null;
        private Layer m_Layer_Overlay = null;
        private bool m_KeepSwimming = true;
        private SKPoint m_MousePos = new SKPoint();
        private bool m_ShowGrid = true;
        private SKPoint m_PrevMouseLoc = new SKPoint();

        SKSize ViewSize;
#if __ANDROID__
        public LayerView(Context context) : this(context, null)
        {

        }

        public LayerView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initial();
        }

        public LayerView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initial();
        }

        private void Initial()
        {
            this.ViewAttachedToWindow += (sender, e) =>
            {
                Initialize();
            };
            this.ViewDetachedFromWindow += (sender, e) =>
            {
                // Let the rendering thread terminate
                m_KeepSwimming = false;
                //m_ThreadGate.Set();
            };
        }
#else
        public LayerView() : base()
        {
            Initialize();
        } 
#endif
        private void Initialize()
        {
            // Create layers to draw on, each with a dedicated SKPicture
            m_Layer_Background = new Layer("Background Layer");
            m_Layer_Grid = new Layer("Grid Layer");
            m_Layer_Data = new Layer("Data Layer");
            m_Layer_Overlay = new Layer("Overlay Layer");

            // Create a collection for the drawing layers
            m_Layers = new List<Layer>();
            m_Layers.Add(m_Layer_Background);
            m_Layers.Add(m_Layer_Grid);
            m_Layers.Add(m_Layer_Data);
            m_Layers.Add(m_Layer_Overlay);

            // Subscribe to the Draw Events for each layer
            m_Layer_Background.Draw += Layer_Background_Draw;
            m_Layer_Grid.Draw += Layer_Grid_Draw;
            m_Layer_Data.Draw += Layer_Data_Draw;
            m_Layer_Overlay.Draw += Layer_Overlay_Draw;

            // Subscribe to the touch events
#if __ANDROID__
            //android中触摸位置只能在Touch事件中获得,移动在触摸事件中处理,点击有单独Click事件处理
            this.Touch += LayerView_Touch;
            this.Click += LayerView_Click;
#elif __IOS__
            //ios中直接使用Touch方法完成点击和移动事件
#elif __UWP__
            this.PointerMoved += (sender, e) =>
            {
                var pointer = e.GetCurrentPoint(this);
                TouchPoint = pointer.Position.ToSKPoint();
                LayerView_Moved();
            };
            this.DoubleTapped += (sender, e) =>
            {
                var pointer = e.GetPosition(this);
                TouchPoint = pointer.ToSKPoint();
                LayerView_DoubleClick(this, null);
            };
            this.SizeChanged += (sender, e) =>
            {
                  ViewSize = new SKSize((float)ActualWidth, (float)ActualHeight);
                  // Invalidate all of the Layers
                  foreach (var layer in m_Layers)
                  {
                      layer.Invalidate();
                  }

                  // Start a new rendering cycle to redraw all of the layers.
                  UpdateDrawing();
            };
#elif __WPF__
            this.MouseMove += (sender, e) =>
            {
                var pointer = e.GetPosition(this);
                TouchPoint = new SKPoint((float)pointer.X,(float)pointer.Y);
                LayerView_Moved();
            };
            this.SizeChanged += (sender, e) =>
            {
                ViewSize = new SKSize((float)ActualWidth, (float)ActualHeight);
                // Invalidate all of the Layers
                foreach (var layer in m_Layers)
                {
                    layer.Invalidate();
                }

                // Start a new rendering cycle to redraw all of the layers.
                UpdateDrawing();
            };
#endif
            this.PaintSurface += LayerView_PaintSurface;

            // Create a background rendering thread
            //m_RenderThread = new Thread(RenderLoopMethod);
            //m_ThreadGate = new AutoResetEvent(false);

            // Start the rendering thread
            // m_RenderThread.Start();

#if __ANDROID__
            ViewSize = new SKSize((float)Width, (float)Height);
#elif __IOS__
            ViewSize = new SKSize((float)this.Frame.Width, (float)this.Frame.Height);
#elif __UWP__
            ViewSize = new SKSize((float)ActualWidth, (float)ActualHeight);
#elif __WPF__
            ViewSize = new SKSize((float)ActualWidth, (float)ActualHeight);
#endif
            System.Diagnostics.Debug.WriteLine(ViewSize);
        }


        SKPoint TouchPoint;
        DateTime DateTime;
        int ClickCount = 0;
#if __ANDROID__
        private void LayerView_Touch(object sender, TouchEventArgs e)
        {
            TouchPoint = new SKPoint(e.Event.GetX(), e.Event.GetY());
            base.OnTouchEvent(e.Event);
            if (e.Event.Action == MotionEventActions.Move)
            {
                LayerView_Moved();
            }
        }

        private void LayerView_Click(object sender, EventArgs e)
        {
            ClickCount++;
            Task.Run(() =>
            {
                Thread.Sleep(300);
                if (ClickCount == 1)
                {
                }
                else if (ClickCount == 2)
                {
                    LayerView_DoubleClick(sender, e);
                }
                ClickCount = 0;
            });
        }
#elif __IOS__
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            UITouch touch = touches.AnyObject as UITouch;
            TouchPoint = touch.LocationInView(this).ToSKPoint();
            if (touch != null)
            {
                if (ClickCount == 1)
                {
                }
                if (touch.TapCount == 2)
                {
                    // do something with the double touch.
                    LayerView_DoubleClick(this, null);
                }
            }
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            UITouch touch = touches.AnyObject as UITouch;
            TouchPoint = touch.LocationInView(this).ToSKPoint();
            LayerView_Moved();
        }
#endif
        private void LayerView_Moved()
        {
            // Save the mouse position
            m_MousePos = TouchPoint;

            // If Left-Click Drag, draw new bars
            /*if (pointer.PointerDevice.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                if (pointer.Properties.IsLeftButtonPressed)
                    // Invalidate the Data Layer to draw a new random set of bars
                    m_Layer_Data.Invalidate();
            }*/

            //安卓上不做左键判断
            // Invalidate the Data Layer to draw a new random set of bars
            m_Layer_Data.Invalidate();

            // If Mouse Move, draw new mouse coordinates
            if (TouchPoint != m_PrevMouseLoc)
            {
                // Remember the previous mouse location
                m_PrevMouseLoc = TouchPoint;

                // Invalidate the Overlay Layer to show the new mouse coordinates
                m_Layer_Overlay.Invalidate();
            }

            // Start a new rendering cycle to redraw any invalidated layers.
            UpdateDrawing();
        }
        private void LayerView_DoubleClick(object sender, EventArgs e)
        {
            // Toggle the grid visibility
            m_ShowGrid = !m_ShowGrid;

            // Invalidate only the Grid Layer.  
            m_Layer_Grid.Invalidate();

            // Start a new rendering cycle to redraw any invalidated layers.
            UpdateDrawing();
        }

        private void LayerView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (e.Info.Width != (int)ViewSize.Width || e.Info.Height != (int)ViewSize.Height)
            {


                // get the screen density for scaling
                float density = 1;
#if __IOS__
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    //iOS中需要在Ui线程中调用,参考https://docs.microsoft.com/zh-cn/xamarin/essentials/device-display?tabs=ios
                    density = (float)DeviceDisplay.MainDisplayInfo.Density;
                });
#else
                density = (float)DeviceDisplay.MainDisplayInfo.Density;
#endif
#if __UWP__
                //var scale = density / 96.0f;//NOTICE:这个式子在UWP上对,但iOS上只是密度,即Dpix1x2x3倍数关系
#endif
                var scale = density;

                var scaledSize = new SKSize(e.Info.Width / scale, e.Info.Height / scale);
                // handle the device screen density
                e.Surface.Canvas.Scale(scale);
            }
            // Clear the Canvas
            e.Surface.Canvas.Clear(SKColors.Black);

            // Paint each pre-rendered layer onto the Canvas using this GUI thread
            foreach (var layer in m_Layers)
            {
                layer.Paint(e.Surface.Canvas);
            }

            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.LimeGreen;

                for (int i = 0; i < m_Layers.Count; i++)
                {
                    var layer = m_Layers[i];
                    var text = $"{layer.Title} - Renders = {layer.RenderCount}, Paints = {layer.PaintCount}";
                    var textLoc = new SKPoint(10, 10 + (i * 15));

                    e.Surface.Canvas.DrawText(text, textLoc, paint);
                }


                paint.Color = SKColors.Cyan;

                e.Surface.Canvas.DrawText("Click-Drag to update bars.", new SKPoint(10, 80), paint);
                e.Surface.Canvas.DrawText("Double-Click to show / hide grid.", new SKPoint(10, 95), paint);
                e.Surface.Canvas.DrawText("Resize to update all.", new SKPoint(10, 110), paint);
            }
        }
#if __ANDROID__
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            //我的
            ViewSize = new SKSize((float)Width, (float)Height);
            // Invalidate all of the Layers
            foreach (var layer in m_Layers)
            {
                layer.Invalidate();
            }

            // Start a new rendering cycle to redraw all of the layers.
            UpdateDrawing();
        }
#elif __IOS__
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (ViewSize.Width != (float)this.Frame.Width || ViewSize.Height != (float)this.Frame.Height)
            {
                ViewSize = ViewSize = new SKSize((float)this.Frame.Width, (float)this.Frame.Height);
            }
            // Invalidate all of the Layers
            foreach (var layer in m_Layers)
            {
                layer.Invalidate();
            }

            // Start a new rendering cycle to redraw all of the layers.
            UpdateDrawing();
        }
#elif __UWP__

#endif
        public void UpdateDrawing()
        {
            // Unblock the rendering thread to begin a render cycle.  Only the invalidated
            // Layers will be re-rendered, but all will be repainted onto the SKGLControl.
            //m_ThreadGate.Set();

            RenderLoopMethod();
        }

        private void RenderLoopMethod()
        {
            //while (m_KeepSwimming)
            //{
            // Draw any invalidated layers using this Render thread
            DrawLayers();

            // Invalidate the SKGLControl to run the PaintSurface event on the GUI thread
            // The PaintSurface event will Paint the layer stack to the SKGLControl
#if __ANDROID__
            if (MainThread.IsMainThread)
                Invalidate();
            else
                PostInvalidate();
#elif __IOS__
            this.SetNeedsDisplay();
#elif __UWP__
            this.Invalidate();
#elif __WPF__
            this.InvalidateVisual();
#endif
            // DoEvents to ensure that the GUI has time to process
            // Application.DoEvents();

            // Block and wait for the next rendering cycle
            //    m_ThreadGate.WaitOne();
            //}
        }


        // -------------------
        // --- Draw Layers ---
        // -------------------

        private void DrawLayers()
        {
            // Iterate through the collection of layers and raise the Draw event for each layer that is
            // invalidated.  Each event handler will receive a Canvas to draw on along with the Bounds for 
            // the Canvas, and can then draw the contents of that layer. The Draw commands are recorded and  
            // stored in an SKPicture for later playback to the SKGLControl.  This method can be called from
            // any thread.

            //var clippingBounds = skglControl1.ClientRectangle.ToSKRect();
            //var clippingBounds = new SKRect(0,0, (float)skglControl1.ActualWidth,(float)skglControl1.ActualHeight);
            var clippingBounds = new SKRect(0, 0, ViewSize.Width, ViewSize.Height);

            foreach (var layer in m_Layers)
            {
                layer.Render(clippingBounds);
            }
        }


        // -----------------------------------------
        // --- Event - Layer - Background - Draw ---
        // -----------------------------------------

        private void Layer_Background_Draw(object sender, EventArgs_Draw e)
        {
            // Create a diagonal gradient fill from Blue to Black to use as the background
            var topLeft = new SKPoint(e.Bounds.Left, e.Bounds.Top);
            var bottomRight = new SKPoint(e.Bounds.Right, e.Bounds.Bottom);
            var gradColors = new SKColor[2] { SKColors.DarkBlue, SKColors.Black };

            using (var paint = new SKPaint())
            using (var shader = SKShader.CreateLinearGradient(topLeft, bottomRight, gradColors, SKShaderTileMode.Clamp))
            {
                paint.Shader = shader;
                paint.Style = SKPaintStyle.Fill;
                e.Canvas.DrawRect(e.Bounds, paint);
            }
        }


        // -----------------------------------
        // --- Event - Layer - Grid - Draw ---
        // -----------------------------------

        private void Layer_Grid_Draw(object sender, EventArgs_Draw e)
        {
            if (m_ShowGrid)
            {
                // Draw a 25x25 grid of gray lines

                using (var paint = new SKPaint())
                {
                    paint.Color = new SKColor(64, 64, 64); // Very dark gray
                    paint.Style = SKPaintStyle.Stroke;
                    paint.StrokeWidth = 1;

                    // Draw the Horizontal Grid Lines
                    var vGridCount = (int)(e.Bounds.Height / 5);
                    for (int i = 0; i < vGridCount; i++)
                    {
                        var y = e.Bounds.Height * (i*1f / vGridCount);
                        var leftPoint = new SKPoint(e.Bounds.Left, y);
                        var rightPoint = new SKPoint(e.Bounds.Right, y);

                        e.Canvas.DrawLine(leftPoint, rightPoint, paint);
                    }
                    var hGridCount  = (int)(e.Bounds.Width / 5);
                    // Draw the Vertical Grid Lines
                    for (int i = 0; i < hGridCount; i++)
                    {
                        var x = e.Bounds.Width * (i*1f / hGridCount);
                        var topPoint = new SKPoint(x, e.Bounds.Top);
                        var bottomPoint = new SKPoint(x, e.Bounds.Bottom);

                        e.Canvas.DrawLine(topPoint, bottomPoint, paint);
                    }
                }
            }
        }


        // -----------------------------------
        // --- Event - Layer - Date - Draw ---
        // -----------------------------------

        private void Layer_Data_Draw(object sender, EventArgs_Draw e)
        {
            // Draw a simple bar graph

            // Flip the Y-Axis so that zero is on the bottom
            e.Canvas.Scale(1, -1);
            e.Canvas.Translate(0, -e.Bounds.Height);

            var rand = new Random();

            // Create 25 red / yellow gradient bars of random length

            var hGridCount = (int)(e.Bounds.Width / 5);

            for (int i = 0; i < hGridCount; i++)
            {
                var barWidth = e.Bounds.Width / hGridCount;
                var barHeight = rand.Next((int)(e.Bounds.Height * 0.65d));
                var barLeft = (i + 0) * barWidth;
                var barRight = (i + 1) * barWidth;
                var barTop = barHeight;
                var barBottom = 0;
                var topLeft = new SKPoint(barLeft, barTop);
                var bottomRight = new SKPoint(barRight, barBottom);
                var gradColors = new SKColor[2] { SKColors.Yellow, SKColors.Red };

                // Draw each bar with a gradient fill
                using (var paint = new SKPaint())
                using (var shader = SKShader.CreateLinearGradient(topLeft, bottomRight, gradColors, SKShaderTileMode.Clamp))
                {
                    paint.Style = SKPaintStyle.Fill;
                    paint.StrokeWidth = 1;
                    paint.Shader = shader;

                    e.Canvas.DrawRect(barLeft, barBottom, barWidth, barHeight, paint);
                }

                // Draw the border of each bar
                using (var paint = new SKPaint())
                {
                    paint.Color = SKColors.Blue;
                    paint.Style = SKPaintStyle.Stroke;
                    paint.StrokeWidth = 1;

                    e.Canvas.DrawRect(barLeft, barBottom, barWidth, barHeight, paint);
                }
            }
        }


        // --------------------------------------
        // --- Event - Layer - Overlay - Draw ---
        // --------------------------------------

        private void Layer_Overlay_Draw(object sender, EventArgs_Draw e)
        {
            // Draw the mouse coordinate text next to the cursor

            using (var paint = new SKPaint())
            {
                // Configure the Paint to draw a black rectangle behind the text
                paint.Color = SKColors.Black;
                paint.Style = SKPaintStyle.Fill;

                // Measure the bounds of the text
                var text = m_MousePos.ToString();
                SKRect textBounds = new SKRect();
                paint.MeasureText(text, ref textBounds);

                // Fix the inverted height value from the MeaureText
                textBounds = textBounds.Standardized;
                textBounds.Location = new SKPoint(m_MousePos.X, m_MousePos.Y - textBounds.Height);

                // Draw the black filled rectangle where the text will go
                e.Canvas.DrawRect(textBounds, paint);

                // Change the Paint to yellow
                paint.Color = SKColors.Yellow;

                // Draw the mouse coordinates text
                e.Canvas.DrawText(m_MousePos.ToString(), m_MousePos, paint);
            }
        }
    }

}
