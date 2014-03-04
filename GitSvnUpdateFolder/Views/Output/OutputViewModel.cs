using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Events;
using GitSvnUpdateFolder.Events;

namespace GitSvnUpdateFolder.Views.Output
{
    public class OutputViewModel : NotificationObject, IOutputViewModel
    {
        public OutputViewModel(IOutputView view, IEventAggregator eventAggregator)
        {
            View = view;
            View.DataContext = this;

            eventAggregator.GetEvent<FolderItemSelectedEvent>().Subscribe(f =>
            {
                Output = f != null ? f.Output : null;
            });
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
