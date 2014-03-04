using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GitSvnUpdateFolder.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;

namespace GitSvnUpdateFolder.Views.Batches
{
    public class BatchesViewModel : NotificationObject, IBatchesViewModel
    {
        public BatchesViewModel(IBatchesView view, IEventAggregator eventAggregator)
        {
            View = view;
            View.DataContext = this; 
            
            eventAggregator.GetEvent<FolderItemSelectedEvent>().Subscribe(f =>
            {
                List = f != null ? f.Batches : null;
            });
        }

        public IBatchesView View { get; set; }

        private ObservableCollection<string> _List;
        public ObservableCollection<string> List
        {
            get
            {
                return _List;
            }
            set
            {
                _List = value;
                RaisePropertyChanged(() => List);
            }
        }
    }
}
