using ReloadPreview;
using SharpConstraintLayout.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xamarin.Helper.Logs;
using XGallery.Views;

namespace XGallery.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IReload
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Content = new ConstraintLayout();
            Reload(this, Content);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            ReloadClient.GlobalInstance.Reload += GlobalInstance_Reload;
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            LogHelper.Debug("MainWindow OnInitialized");
            
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            LogHelper.Debug("MainWindow OnClosed");
            ReloadClient.GlobalInstance.Reload -= GlobalInstance_Reload;
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            LogHelper.Debug("MainWindow OnActivated");
            //ReloadClient.GlobalInstance.Reload += GlobalInstance_Reload;
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            LogHelper.Debug("MainWindow OnDeactivated");
            //ReloadClient.GlobalInstance.Reload -= GlobalInstance_Reload;
        }

        private void GlobalInstance_Reload()
        {
            LogHelper.Debug("MainWindow GlobalInstance_Reload");
            ReloadClient.GlobalInstance.ReloadType<MainWindow>(this, Content);
        }

        public void Reload(object controller, object view)
        {
            InitPage(controller as Window, view as ConstraintLayout);
        }

        private void InitPage(Window window, ConstraintLayout ContentView)
        {
            ContentView.Children.Clear();
            LogHelper.Debug("{0},{1}", this.GetType().Name, nameof(InitPage));
            ContentView.Background = new SolidColorBrush(Colors.Cyan);
            var view = new LayerView_M();
            ContentView.Children.Add(view);
            view.CenterTo(ContentView) 
                .WidthEqualTo(ConstraintSet.SizeType.MatchConstraint)
                .HeightEqualTo(ConstraintSet.SizeType.MatchConstraint);
        }
    }
}
