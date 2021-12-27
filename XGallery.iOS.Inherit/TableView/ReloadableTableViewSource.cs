using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace XGallery.iOS.Inherit.TableView
{
    /// <summary>
    /// For Reload
    /// </summary>
    public class ReloadableTableViewSource : UITableViewSource
    {
        public ITableViewSource ReloadSource { private set;  get; }
        public ReloadableTableViewSource(ITableViewSource viewSource)
        {
           this.ReloadSource = viewSource;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            return ReloadSource.GetCell(tableView, indexPath);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ReloadSource.RowsInSection(tableview, section);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowSelected(tableView, indexPath);
            ReloadSource.RowSelected(tableView, indexPath);
        }
    }
}