
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace XGallery.Share
{
    public class TaskPageViewModel : ObservableObject
    {
        private ObservableCollection<TaskItemViewModel> taskItems;
        public ObservableCollection<TaskItemViewModel> TaskItems
        {
            get => taskItems;
            set => SetProperty(ref taskItems, value);
        }
    }
}