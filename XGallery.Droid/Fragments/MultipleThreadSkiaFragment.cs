
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Helper.Controllers;
using XGallery.Views;

namespace XGallery.Droid.Fragments
{
    public class MultipleThreadSkiaFragment : BaseFragment,IReload
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
            ReloadClient.GlobalInstance.ReloadType<MultipleThreadSkiaFragment>(this, ContentView);
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

        private void InitPage(BaseFragment fragment, ConstraintLayout ContentView)
        {
            ContentView.RemoveAllViews();
            ContentView.SetBackgroundColor(Color.Beige);
            var list = new LayerView(ContentView.Context) { Id = View.GenerateViewId() };
            ContentView.AddView(list, new ConstraintLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            
        }
    }
}