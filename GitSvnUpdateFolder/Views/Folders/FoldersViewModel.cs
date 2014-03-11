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
        private bool _running = false;

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
                    },
                f => !_running);

            RebaseCommand = new DelegateCommand<FolderModel>(
                f =>  
                    {
                        SelectedFolder = f;
                        _eventAggregator.GetEvent<RebaseFolderEvent>().Publish(f);
                    },
                f => !_running);

            CommitCommand = new DelegateCommand<FolderModel>(
                f =>  
                    {
                        SelectedFolder = f;
                        _eventAggregator.GetEvent<CommitFolderEvent>().Publish(f);
                    },
                f => !_running);

            GitExtensionsCommand = new DelegateCommand<FolderModel>(
               f =>  
                    {
                        SelectedFolder = f;
                        _eventAggregator.GetEvent<GitExtentionsEvent>().Publish(f);
                    });

            _eventAggregator.GetEvent<ProcessStartEvent>().Subscribe(obj => UpdateCommands(true));
            _eventAggregator.GetEvent<ProcessEndEvent>().Subscribe(obj => UpdateCommands(false));
        }

        private void UpdateCommands(bool running)
        {
            _running = running;
            FetchCommand.RaiseCanExecuteChanged();
            RebaseCommand.RaiseCanExecuteChanged();
            CommitCommand.RaiseCanExecuteChanged();
            //GitExtensionsCommand.RaiseCanExecuteChanged();
        }

        public IFoldersView View { get; set; }
        public DelegateCommand<FolderModel> FetchCommand { get; set; }
        public DelegateCommand<FolderModel> RebaseCommand { get; set; }
        public DelegateCommand<FolderModel> CommitCommand { get; set; }
        public DelegateCommand<FolderModel> GitExtensionsCommand { get; set; }

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
