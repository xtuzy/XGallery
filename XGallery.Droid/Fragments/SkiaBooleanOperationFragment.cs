
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Fragment.App;
using ReloadPreview;
using SkiaSharp;
using SkiaSharp.Views.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Helper.Controllers;

namespace XGallery.Droid.Fragments
{
    public class SkiaBooleanOperationFragment : BaseFragment,IReload
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view =  base.OnCreateView(inflater, container, savedInstanceState);
            InitPage(this, ContentView);
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            ReloadClient.GlobalInstance.Reload += GlobalInstance_Reload;
        }

        private void GlobalInstance_Reload()
        {
            ReloadClient.GlobalInstance.ReloadType<SkiaBooleanOperationFragment>(this, ContentView);
        }

        public override void OnStop()
        {
            base.OnStop(); 
            ReloadClient.GlobalInstance.Reload -= GlobalInstance_Reload;
        }

        public void Reload(object controller, object view)
        {
            InitPage(controller as BaseFragment, view as ConstraintLayout);
        }


        private void InitPage(BaseFragment fragment,ConstraintLayout ContentView)
        {
            ContentView.RemoveAllViews();
            ContentView.SetBackgroundColor(Color.Gray);
            var canvas = new SKCanvasView(ContentView.Context) { Id = View.GenerateViewId() };
            ContentView.AddView(canvas, new ConstraintLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            canvas.PaintSurface += Canvas_PaintSurface;
        }
        bool isDrawShadow = false;
        bool isDrawBlur = true;
        bool isDrawCover = false;
        private void Canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear();//SKColor.Parse("#1b0c66"));
            var w = e.Info.Width;
            var h = e.Info.Height;

            //圆
            if (isDrawBlur)
            {
                var dstBitmap = new SKBitmap(w * 4 / 5, h * 4 / 5);

                using (SKPaint paint = new SKPaint())
                {
                    using (SKCanvas dstcanvas = new SKCanvas(dstBitmap))
                    {
                        dstcanvas.Clear();
                        // Set SKPaint properties
                        // Create linear gradient from upper-left to lower-right
                        paint.TextSize = 150;
                        paint.StrokeWidth = 5;
                        paint.Color = SKColors.Red;
                        dstcanvas.DrawText("Hello", 0, h * 4 / 5 / 2, paint);
                        paint.Shader = SKShader.CreateLinearGradient(
                                            new SKPoint(0, h * 4 / 5 / 2),
                                            new SKPoint(w * 4 / 5 * 2, h * 4 / 5 / 2),
                                            new SKColor[] { SKColors.Red, SKColors.Blue },
                                            new float[] { 0, 1 },
                                            SKShaderTileMode.Repeat);
                        //paint.ImageFilter = SKImageFilter.CreateBlur(10, 10);
                        dstcanvas.DrawCircle(w * 4 / 5, h * 4 / 5 / 2, w * 4 / 5 / 2, paint);
                    }
                }

                canvas.DrawBitmap(dstBitmap, w / 5 / 2, h / 5 / 2);
            }


            //模糊矩形框
            if (isDrawBlur)
            {
                // Get values from sliders c
                float sigmaX = 0;
                float sigmaY = 0;

                using (SKPaint paint = new SKPaint())
                {
                    // Set SKPaint properties
                    paint.ImageFilter = SKImageFilter.CreateBlur(10, 10);
                    paint.Color = SKColors.White.WithAlpha(256 * 20 / 100);
                    float baseFreqX = (float)Math.Pow(10, 1);
                    float baseFreqY = (float)Math.Pow(10, 1);
                    int numOctaves = (int)5;
                    /* paint.Shader = SKShader.CreatePerlinNoiseFractalNoise(baseFreqX,
                                                       baseFreqY,
                                                       numOctaves,
                                                       0);*/
                    paint.BlendMode = SKBlendMode.SrcOver;

                    canvas.DrawRoundRect(new SKRect(w / 5 / 2, h / 5 / 2, w * 4 / 5 + w / 5 / 2, h * 4 / 5 + h / 5 / 2), 50, 50, paint);
                }
            }
        }
    }
}