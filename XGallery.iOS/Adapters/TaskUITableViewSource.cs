using Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Helper.Layouts;
using Xamarin.Helper.Logs;
using XGallery.iOS.Inherit.TableView;
using XGallery.Share;

namespace XGallery.iOS.Adapters
{
    internal class TaskUITableViewSource : ITableViewSource
    {
        int PreviewSelectedCell;
        int CurrentSelectedCell;

        //public event EventHandler<MyRecycleAdapterClickEventArgs> ItemClick;
        public event EventHandler<MyTableViewSourceClickEventArgs> ItemLongClick;
        public event EventHandler<MyTableViewSourceClickEventArgs> ButtonClick;
        void OnButtonClick(MyTableViewSourceClickEventArgs args) => ButtonClick?.Invoke(this, args);

        ObservableCollection<TaskItemViewModel> items;

        public TaskUITableViewSource(ObservableCollection<TaskItemViewModel> taskItems)
        {
            items = taskItems;
            ButtonClick += TaskUITableViewSource_ButtonClick;
        }

        private void TaskUITableViewSource_ButtonClick(object sender, MyTableViewSourceClickEventArgs e)
        {
            items.Insert(e.Position, new TaskItemViewModel() { Title = items[e.Position].Title + "A", Time = DateTime.Now });
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("CellIdentifier") as ReloadableTableViewCell;
            var item = items[indexPath.Row];
            //LogHelper.Debug($"GetCell {indexPath.Row}");
            //if there are no cells to reuse, create a new one
            if (cell == null)
            {
                //cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                cell = new ReloadableTableViewCell(new TaskUITableViewCell(OnButtonClick), UITableViewCellStyle.Default, (NSString)"CellIdentifier");
            }
            else//取消之前的事件订阅
            {
                //(cell.ReloadCell as TaskUITableViewCell).Do.TouchUpInside -= Do_TouchUpInside;
            }

            var cellView = cell.ReloadCell as TaskUITableViewCell;
            cellView.Title.Text = item.Title;
            cellView.Description.Text = item.Description;
            cellView.Time.Text = item.Time.ToString();

            cellView.Do.SetTitle($"Index {indexPath.Row}", UIControlState.Normal);
            cellView.Do.Tag = indexPath.Row;
            if (PreviewSelectedCell != default)//折叠
            {
                if(PreviewSelectedCell == indexPath.Row)
                {
                    var previewCell = (tableView.CellAt(indexPath) as ReloadableTableViewCell).ReloadCell as TaskUITableViewCell;
                    previewCell.Description.Hidden = true;
                }
            } 
            if(CurrentSelectedCell != default)//展开
            {
                if (CurrentSelectedCell == indexPath.Row)
                {
                    cellView.Description.Hidden = false;
                }
            }
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return items.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            PreviewSelectedCell = CurrentSelectedCell;
            CurrentSelectedCell = indexPath.Row;
            tableView.ReloadRows(new[] { indexPath }, UITableViewRowAnimation.Automatic);
        }

        internal class TaskUITableViewCell : ITableViewCell
        {
            static int index = 0;
            public UILabel Title;
            public UILabel Time;
            public UIButton Do;
            public UILabel Description;
            public TaskUITableViewCell(Action<MyTableViewSourceClickEventArgs> onButtonClick)
            {
                //LogHelper.Debug($"CreateCell {index++}");
                Title = new UILabel() { TranslatesAutoresizingMaskIntoConstraints = false };
                Description = new UILabel() { TranslatesAutoresizingMaskIntoConstraints = false };
                Time = new UILabel() { TranslatesAutoresizingMaskIntoConstraints = false };
                Do = new UIButton(UIButtonType.RoundedRect) { TranslatesAutoresizingMaskIntoConstraints = false };
                Do.TouchUpInside += (sender,e) => onButtonClick(new MyTableViewSourceClickEventArgs() { View = Do, Position = (int)Do.Tag });
                Description.Hidden = true;
            }

            public override void LayoutSubviews(ReloadableTableViewCell NativeCell)
            { 
                NativeCell.AddSubviews(Title);
                NativeCell.AddSubviews(Description);
                NativeCell.AddSubviews(Time);
                NativeCell.AddSubviews(Do);
                Title.CenterYTo(NativeCell).LeftToLeft(NativeCell, 20);
                Time.CenterTo(NativeCell);
                Do.CenterYTo(NativeCell).RightToRight(NativeCell, -20);
                Description.TopToBottom(Title,20).LeftToRight(Title, 20).BottomToBottom(NativeCell);
            }
        }

        /// <summary>
        /// 自定义事件参数
        /// </summary>
        internal class MyTableViewSourceClickEventArgs : EventArgs
        {
            public UIView View { get; set; }
            public int Position { get; set; }
        }
    }
}