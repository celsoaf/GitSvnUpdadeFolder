using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using GitSvnUpdateFolder.Models;
using Microsoft.Practices.Prism.Events;
using GitSvnUpdateFolder.Events;

namespace GitSvnUpdateFolder.Views.Folders
{
    public class FoldersViewModel : IFoldersViewModel
    {
        private IEventAggregator _eventAggregator;

        public FoldersViewModel(IFoldersView view, IEventAggregator eventAggregator)
        {
            View = view;
            View.DataContext = this;

            _eventAggregator = eventAggregator;

            Folders = new ObservableCollection<FolderModel>();
        }

        public IFoldersView View { get; set; }


        public ObservableCollection<FolderModel> Folders { get; set; }
        private FolderModel _SelectedFolder;
        public FolderModel SelectedFolder
        {
            get
            {
                return _SelectedFolder;
            }
            set
            {
                _SelectedFolder = value;
                
                _eventAggregator.GetEvent<FolderItemSelectedEvent>().Publish(value);
            }
        }
    }
}
