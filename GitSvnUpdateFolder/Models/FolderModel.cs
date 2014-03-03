using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSvnUpdateFolder.Enums;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;

namespace GitSvnUpdateFolder.Models
{
    public class FolderModel : NotificationObject
    {
        public FolderModel()
        {
            Output = new ObservableCollection<string>();
        }

        public string Name { get; set; }
        public string FullPath { get; set; }
        private FolderState _State;
        public FolderState State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
                RaisePropertyChanged(() => State);
            }
        }

        public ObservableCollection<string> Output { get; set; }
    }
}
