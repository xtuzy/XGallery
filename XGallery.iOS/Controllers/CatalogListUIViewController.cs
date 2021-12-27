using CoreFoundation;
using Foundation;
using ReloadPreview;
using System;
using System.Drawing;
using UIKit;
using Xamarin.Helper.Controllers;
using Xamarin.Helper.Logs;

namespace XGallery.iOS.Controllers
{

    [Register("CatalogListUIViewController")]
    public class CatalogListUIViewController : BaseUIViewController
    {
        public CatalogListUIViewController()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            View = new UIView();

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
            LogHelper.Debug("{0},{1}", this.GetType().Name, nameof(ViewDidLoad));
            new CatalogListUIViewController_Reload().Reload(this, View);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ReloadClient.GlobalInstance.Reload += ReloadClient_Reload;
        }

        private void ReloadClient_Reload()
        {
            ReloadClient.GlobalInstance.ReloadType<CatalogListUIViewController_Reload>(this, View);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ReloadClient.GlobalInstance.Reload -= ReloadClient_Reload;
        }

       /* public void Reload(object controller, object view)
        {
            try
            {
                InitPage(controller as UIViewController, view as UIView);
            }catch (Exception ex)
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
          ContentView.BackgroundColor = UIColor.Yellow;

            UIStackView stackView = new UIStackView(new Rectangle(0,0,500,500));
            ContentView.Add(stackView);
            var button = new UIButton(UIButtonType.RoundedRect);
            button.SetTitle("Skia",UIControlState.Normal);
            stackView.Add(button);
            button.TouchUpInside += (sender, e) =>
            {
                controller.NavigationController.PushViewController(ViewControllerService.NewViewController("SkiaBooleanOperationUIViewController") as UIViewController,false);
            };
        }*/
    }
}