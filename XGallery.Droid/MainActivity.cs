using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Navigation;
using Xamarin.Helper.Controllers;
using Xamarin.Helper.Images;
using XGallery.Droid.Fragments;

namespace XGallery.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : BaseTabBarActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void InitTabFragments()
        {
            TabFragments.Item1 = new CatalogListFragment();
        }

        public override void InitBottomTabBar(BottomNavigationView bottomNavigationView)
        {
            BottomTabBar.Menu.Add((int)Android.Views.MenuAppendFlags.None, 3, (int)Android.Views.MenuAppendFlags.None, new Java.Lang.String("Catalog"))
                .SetIcon(ImageHelper.BitmapToDrawable(this, SvgHelper.FromAssets(this, "ic_task_pressed.svg",44)));
            BottomTabBar.ItemTextColor = new ColorStateList(
     new int[][] { new int[] { -Android.Resource.Attribute.StateChecked }, new int[] { Android.Resource.Attribute.StateChecked } },
     new int[] { Color.Gray, Color.Blue });
            BottomTabBar.ItemIconTintList = new ColorStateList(
     new int[][] { new int[] { -Android.Resource.Attribute.StateChecked }, new int[] { Android.Resource.Attribute.StateChecked } },
     new int[] { Color.Gray, Color.Blue });
        }

        public override void BottomTabBar_ItemSelected(object sender, NavigationBarView.ItemSelectedEventArgs e)
        {
            switch ((e.P0 as AndroidX.AppCompat.View.Menu.MenuItemImpl).Title)
            {
                case "Catalog":
                    if (TabFragments.Item1 == null)
                        TabFragments.Item1 = new CatalogListFragment();
                    ShowTabFragment(TabFragments.Item1);
                    this.Title = "Catalog";
                    break;
            }
        }
    }
}