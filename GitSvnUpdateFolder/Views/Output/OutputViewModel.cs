using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;

namespace GitSvnUpdateFolder.Views.Output
{
    public class OutputViewModel : NotificationObject, IOutputViewModel
    {
        public OutputViewModel(IOutputView view)
        {
            View = view;
            View.DataContext = this;
        }

        public IOutputView View { get; set; }

        private ObservableCollection<string> _Output;
        public ObservableCollection<string> Output
        {
            get
            {
                return _Output;
            }
            set
            {
                _Output = value;
                RaisePropertyChanged(() => Output);
            }
        }
    }
}
