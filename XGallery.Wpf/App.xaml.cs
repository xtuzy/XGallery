using ReloadPreview;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Helper.Logs;
using Xamarin.Helper.Tools;

namespace XGallery.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // class-level declarations
        const string TAG = "XGallery App";

        public static string IP = "192.168.0.108";//"172.25.96.1";//
        public static int Port = 400;

        protected override void OnStartup(StartupEventArgs e)
        {
            Debug.WriteLine("OnStartup");
            base.OnStartup(e);

            ReloadClient.GlobalInstance = new ReloadClient(IP, Port);
            ReloadClient.GlobalInstance.Start();

            var client = new MessageClientApp(IP);
            LogHelper.DebugEvent += (message) =>
            {
                Debug.WriteLine(message);
                client.SendMessage(message + "~Windows" + $"~{DateTime.Now}" + "\n");//~为间隔
            };
        }

        protected override void OnActivated(EventArgs e)
        {
            LogHelper.Debug("App OnActivated");
            base.OnActivated(e);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            LogHelper.Debug("App OnDeactivated");
            base.OnDeactivated(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
