
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace XGallery.Share
{
    public class TaskItemViewModel : ObservableObject
    {

        private DateTime time;
        public DateTime Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }


        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }


        private string description;

        public TaskItemViewModel()
        {

        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        ICommand SaveCommand { get; set; }
    }
}