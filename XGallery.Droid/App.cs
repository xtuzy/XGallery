using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Lifecycle;
using Java.Interop;
using ReloadPreview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Helper.Controllers;
using Xamarin.Helper.Logs;
using Xamarin.Helper.Tools;
using XGallery.Droid.Fragments;

namespace XGallery.Droid
{
    [Application]
    public partial class App : BaseApplication, ILifecycleObserver
    {
        const string TAG = "XGallery App";

        public static string IP = "192.168.0.108";//"172.25.96.1";//
        public static int Port = 300;
        public App(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
        }

        public override void OnCreate()
        {
            ReloadClient.GlobalInstance = new ReloadClient(IP, Port);
            ReloadClient.GlobalInstance.Start();

            var client = new MessageClientApp(IP);
            LogHelper.DebugEvent += (message) =>
            {
                client.SendMessage(message + "~Android" + $"~{DateTime.Now}" + "\n");//~为间隔
            };
            
            base.OnCreate();
            ProcessLifecycleOwner.Get().Lifecycle.AddObserver(this);
            LogHelper.Debug("{0} {1}", nameof(App), nameof(OnCreate));

            ReloadPreview.ViewControllerService.RecordViewController(nameof(SkiaBooleanOperationFragment), typeof(SkiaBooleanOperationFragment));
            ReloadPreview.ViewControllerService.RecordViewController(nameof(MultipleThreadSkiaFragment), typeof(MultipleThreadSkiaFragment));
           
        }


        [Lifecycle.Event.OnStart]
        [Export]
        public void OnStart()
        {
            LogHelper.Debug("{0} {1}", nameof(App), nameof(OnStart));
        }

        [Lifecycle.Event.OnPause]
        [Export]
        public void OnPause()
        {
            LogHelper.Debug("{0} {1}", nameof(App), nameof(OnPause));
        }

        [Lifecycle.Event.OnResume]
        [Export]
        public void OnResume()
        {
            LogHelper.Debug("{0} {1}", nameof(App), nameof(OnResume));
        }

        [Lifecycle.Event.OnStop]
        [Export]
        public void OnStop()
        {
            LogHelper.Debug("{0} {1}", nameof(App), nameof(OnStop));
        }

        [Lifecycle.Event.OnDestroy]
        [Export]
        public void OnDestroy()
        {
            LogHelper.Debug("{0} {1}", nameof(App), nameof(OnDestroy));
        }

    }
}