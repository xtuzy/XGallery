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

    [Register("SkiaBooleanOperationUIViewController")]
    public class SkiaBooleanOperationUIViewController : BaseUIViewController
    {
        public SkiaBooleanOperationUIViewController()
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
            new SkiaBooleanOperationUIViewController_Reload().Reload(this, View);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ReloadClient.GlobalInstance.Reload += ReloadClient_Reload;
        }

        private void ReloadClient_Reload()
        {
            ReloadClient.GlobalInstance.ReloadType<SkiaBooleanOperationUIViewController_Reload>(this, View);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ReloadClient.GlobalInstance.Reload -= ReloadClient_Reload;
        }


    }
}