using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace XGallery.iOS.Inherit.TableView
{
    [Register("ReloadableTableViewCell")]
    public class ReloadableTableViewCell : UITableViewCell
    {
        /// <summary>
        /// 标记Cell在TableView中位置,便于需要位置时使用(尤其是在Cell中的控件触发的事件中需要时)
        /// </summary>
        public int Position;

        public ITableViewCell ReloadCell { private set; get; }

        public ReloadableTableViewCell(ITableViewCell cell)
        {
            ReloadCell = cell;
            ReloadCell.NativeCell = this;
        }

        public ReloadableTableViewCell(ITableViewCell cell,UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
        {
            ReloadCell = cell;
        }

        public override void LayoutSubviews()
        {
            ReloadCell.LayoutSubviews(this);
            base.LayoutSubviews();
        }

    }
}