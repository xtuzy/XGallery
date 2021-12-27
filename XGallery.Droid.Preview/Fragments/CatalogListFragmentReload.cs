using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using ReloadPreview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Helper.Controllers;
using Xamarin.Helper.Logs;

namespace XGallery.Droid.Preview.Fragments
{
    public class CatalogListFragmentReload : IReload
    {
        BaseFragment Fragment;
        ConstraintLayout ContentView;
        public void Reload(object controller, object view)
        {
            LogHelper.Debug(this.GetType().Name);
            Fragment = controller as BaseFragment;
            ContentView = (ConstraintLayout)view;
            ContentView.RemoveAllViews();
            InitPage(ContentView);
        }

        private void InitPage(ConstraintLayout ContentView)
        {
            ContentView.SetBackgroundColor(Color.Beige);
            var list =  new LinearLayout(ContentView.Context) {Id=View.GenerateViewId(), Orientation = Orientation.Vertical };
            ContentView.AddView(list, new ConstraintLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            Button b1 = new Button(ContentView.Context) { Text = "SkiaBooleanOperationFragment" };
            list.AddView(b1);
            b1.Click += (s, e) =>
              {
                  Fragment.PushViewController(ViewControllerService.NewViewController("SkiaBooleanOperationFragment") as BaseFragment);
              };
        }
    }
}