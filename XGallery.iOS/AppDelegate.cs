using Foundation;
using ReloadPreview;
using System;
using UIKit;
using Xamarin.Helper.Images;
using Xamarin.Helper.Logs;
using Xamarin.Helper.Tools;
using XGallery.iOS.Controllers;

namespace XGallery.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        const string TAG = "XGallery App";

        public static string IP = "192.168.0.108";//"172.25.96.1";//
        public static int Port = 350;

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            ReloadClient.GlobalInstance = new ReloadClient(IP, Port);
            ReloadClient.GlobalInstance.Start();

            var client = new MessageClientApp(IP);
            LogHelper.DebugEvent += (message) =>
            {
                client.SendMessage(message + "~iOS" + $"~{DateTime.Now}" + "\n");//~为间隔
            };

            // create a new window instance based on the screen size
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            var MainTabBarController = new UITabBarController();

            var catalogListUIViewController = new CatalogListUIViewController() { Title = "Task" };
            var FirstNavigationController = new UINavigationController(catalogListUIViewController);
            FirstNavigationController.TabBarItem = new UITabBarItem("Task", SvgHelper.FromResources("ic_task_pressed.svg", 44), 0);

            MainTabBarController.ViewControllers = new UINavigationController[] { FirstNavigationController};

            Window.RootViewController = MainTabBarController;

            // make the window visible
            Window.MakeKeyAndVisible();
            ReloadPreview.ViewControllerService.RecordViewController(nameof(SkiaBooleanOperationUIViewController), typeof(SkiaBooleanOperationUIViewController));
            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background execution this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transition from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}


