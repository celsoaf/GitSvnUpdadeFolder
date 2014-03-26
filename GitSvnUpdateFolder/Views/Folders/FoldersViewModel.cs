using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using GitSvnUpdateFolder.Models;
using Microsoft.Practices.Prism.Events;
using GitSvnUpdateFolder.Events;
using Microsoft.Practices.Prism.Commands;

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

            FetchCommand = new DelegateCommand<FolderModel>(
                f => 
                    {
                        SelectedFolder = f;
                        _eventAggregator.GetEvent<FetchFolderEvent>().Publish(f);
                    });

            RebaseCommand = new DelegateCommand<FolderModel>(
                f =>  
                    {
                        SelectedFolder = f;
                        _eventAggregator.GetEvent<RebaseFolderEvent>().Publish(f);
                    });

            CommitCommand = new DelegateCommand<FolderModel>(
                f =>  
                    {
                        SelectedFolder = f;
                        _eventAggregator.GetEvent<CommitFolderEvent>().Publish(f);
                    });

            GitExtensionsCommand = new DelegateCommand<FolderModel>(
               f =>
               {
                   SelectedFolder = f;
                   _eventAggregator.GetEvent<GitExtentionsEvent>().Publish(f);
               });

            BrowseCommand = new DelegateCommand<FolderModel>(
               f =>
               {
                   SelectedFolder = f;
                   _eventAggregator.GetEvent<BrowseEvent>().Publish(f.FullPath);
               });
        }

        public IFoldersView View { get; set; }
        public DelegateCommand<FolderModel> FetchCommand { get; set; }
        public DelegateCommand<FolderModel> RebaseCommand { get; set; }
        public DelegateCommand<FolderModel> CommitCommand { get; set; }
        public DelegateCommand<FolderModel> GitExtensionsCommand { get; set; }
        public DelegateCommand<FolderModel> BrowseCommand { get; set; }

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
