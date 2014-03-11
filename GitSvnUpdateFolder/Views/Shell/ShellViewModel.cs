using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSvnUpdateFolder.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;

namespace GitSvnUpdateFolder.Views.Shell
{
    public class ShellViewModel : NotificationObject, IShellViewModel
    {
        public ShellViewModel(IShellView view, IEventAggregator eventAggregator)
        {
            View = view;
            View.DataContext = this;

            eventAggregator.GetEvent<ProcessProgressEvent>().Subscribe(p => ProgressValue = p / 100);
        }

        public IShellView View { get; set; }
        private double _ProgressValue;
        public double ProgressValue
        {
            get
            {
                return _ProgressValue;
            }
            set
            {
                _ProgressValue = value;
                RaisePropertyChanged(() => ProgressValue);
            }
        }
    }
}
