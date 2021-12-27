using Foundation;
using System;
using UIKit;

namespace XGallery.iOS.Inherit.TableView
{
    /// <summary>
    /// For Reload
    /// </summary>
    public abstract class ITableViewSource
    {
        public abstract UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath);
        public abstract nint RowsInSection(UITableView tableview, nint section);
        public abstract void RowSelected(UITableView tableView, NSIndexPath indexPath);
    }
}