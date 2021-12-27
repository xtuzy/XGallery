using Foundation;
using ReloadPreview;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Helper.Layouts;
using Xamarin.Helper.Logs;

namespace XGallery.iOS.Controllers
{
    public class CatalogListUIViewController_Reload : IReload
    {
        public void Reload(object controller, object view)
        {
            try
            {
                InitPage(controller as UIViewController, view as UIView);
            }
            catch (Exception ex)
            {
                LogHelper.Debug(ex.Message);
            }
        }

        private void InitPage(UIViewController controller, UIView ContentView)
        {
            foreach (var view in ContentView.Subviews)
            {
                view.RemoveFromSuperview();
            }

            LogHelper.Debug("{0},{1}", this.GetType().Name, nameof(InitPage));
            ContentView.BackgroundColor = UIColor.White;
            var button = new UIButton(UIButtonType.RoundedRect) { TranslatesAutoresizingMaskIntoConstraints=false};
            button.SetTitle("Skia", UIControlState.Normal);
            ContentView.Add(button);
            button.CenterTo(ContentView);
            button.TouchUpInside += (sender, e) =>
            {
                controller.NavigationController.PushViewController(ViewControllerService.NewViewController("SkiaBooleanOperationUIViewController") as UIViewController, false);
            };
        }
    }
}