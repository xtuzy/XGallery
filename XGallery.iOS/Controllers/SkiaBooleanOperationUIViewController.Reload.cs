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
using XGallery.Share;

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

            tableView = new UITableView() { TranslatesAutoresizingMaskIntoConstraints = false };
            contentView.Add(tableView);
            tableView.CenterTo(contentView).HeightEqualTo(contentView).WidthEqualTo(contentView);

            //尝试在Reload中永久存储ViewModel
            viewModel = ReloadViewModelService.ReloadViewModel<TaskPageViewModel>();
            if (viewModel == null) viewModel = new TaskPageViewModel();
            if (viewModel.TaskItems == null)
            {
                var data = new ObservableCollection<TaskItemViewModel>();
                for (var i = 0; i < 10; i++)
                {
                    data.Add(new TaskItemViewModel() { Title = $"{i}", Time = DateTime.Now, Description = $"Index {i}" });
                }
                viewModel.TaskItems = data;
            }

            var source = new TaskUITableViewSource(viewModel.TaskItems);
            //TODO:Release
            viewModel.TaskItems.CollectionChanged += (sender, e) =>
            {
                ReloadViewModelService.SaveViewModel<TaskPageViewModel>(viewModel);
            };
            viewModel.TaskItems.CollectionChanged += (sender, e) =>
            {

                LogHelper.Debug($"{this.GetType().Name}, Items_CollectionChanged {e.Action}, Count {viewModel.TaskItems.Count}");
                tableView.ReloadData();
            };
            tableView.Source = new ReloadableTableViewSource(source);

            var longPress = new UILongPressGestureRecognizer();
            longPress.AddTarget(() => handleLongPress(longPress));

            tableView.AddGestureRecognizer(longPress);
        }

        private void handleLongPress(UILongPressGestureRecognizer sender)
        {
            LogHelper.Debug("handleLongPress");
            if (sender.State == UIGestureRecognizerState.Began)
            {

                LogHelper.Debug("handleLongPress ");
                var touchPoint = sender.LocationInView(tableView);
                var indexPath = tableView.IndexPathForRowAtPoint(touchPoint);
                // your code here, get the row for the indexPath or do whatever you want
                //viewModel.TaskItems.RemoveAt(indexPath.Row);

                LogHelper.Debug("TabBar" + Controller.NavigationController.TabBarController.TabBar.Frame.Height.ToString());
                LogHelper.Debug("Density" + DeviceDisplay.MainDisplayInfo.Density.ToString());
                LogHelper.Debug("StateBar" + UIApplication.SharedApplication.Windows.First().WindowScene.StatusBarManager.StatusBarFrame.Size.Height.ToString());
                LogHelper.Debug("NavigationBar" + Controller.NavigationController.NavigationBar.Frame.Height.ToString());

                var controller = ViewControllerService.NewViewController("SkiaBooleanOperationUIViewController");
                Controller.NavigationController.PushViewController(controller as UIViewController, false);
            }
        }
    }
}