namespace XGallery.iOS.Inherit.TableView
{
    public abstract class ITableViewCell
    {
        public ReloadableTableViewCell NativeCell;
        public abstract void LayoutSubviews(ReloadableTableViewCell NativeCell);
    }
}