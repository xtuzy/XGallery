using Foundation;
using ReloadPreview;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Helper.Layouts;
using Xamarin.Helper.Logs;
using XGallery.iOS.Adapters;
using XGallery.iOS.Inherit.TableView;
using XGallery.iOS.Inherit.Tools;
using XGallery.Share;
using XGallery.Views;

namespace XGallery.iOS.Controllers
{
    public class SkiaBooleanOperationUIViewController_Reload : IReload
    {
        private UITableView tableView;
        private TaskPageViewModel viewModel;
        private UIViewController Controller;

        public void Reload(object controller, object view)
        {
            LogHelper.Debug(this.GetType().Name);
            var ContentView = view as UIView;
            ContentView.BackgroundColor = UIColor.Blue;
            this.Controller = controller as UIViewController;
            InitPage(controller as UIViewController, ContentView);
        }

        private void InitPage(UIViewController uIViewController, UIView contentView)
        {
            foreach (var view in contentView.Subviews)
            {
                view.RemoveFromSuperview();
            }
            var layerView = new LayerView_M() { TranslatesAutoresizingMaskIntoConstraints=false};
            contentView.AddSubview(layerView);
            layerView.CenterTo(contentView)
                .TopToBottom(uIViewController.NavigationController.NavigationBar)
                .HeightEqualTo(contentView)
                .WidthEqualTo(contentView);
            var fps= new FPSHelper();
            fps.start((a) => { LogHelper.Debug("fps:"+a); });
        }
    }
}