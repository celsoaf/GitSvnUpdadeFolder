using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSvnUpdateFolder.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;

namespace GitSvnUpdateFolder.Views.Progress
{
    public class ProgressViewModel : NotificationObject, IProgressViewModel
    {
        public ProgressViewModel(IProgressView view, IEventAggregator eventAggregator)
        {
            View = view;
            View.DataContext = this;

            eventAggregator.GetEvent<ProcessProgressEvent>().Subscribe(p => Value = p);
        }

        public IProgressView View { get; set; }

        private double _Value;
        public double Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                RaisePropertyChanged(() => Value);
            }
        }
    }
}
