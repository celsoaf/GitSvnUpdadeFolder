﻿using System;
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

            UpdateCommand = new DelegateCommand<FolderModel>(
                f => _eventAggregator.GetEvent<UpdateFolderEvent>().Publish(f),
                f => !_running);

            _eventAggregator.GetEvent<ProcessStartEvent>().Subscribe(obj =>
            {
                _running = true;
                UpdateCommand.RaiseCanExecuteChanged();
            });
            _eventAggregator.GetEvent<ProcessEndEvent>().Subscribe(obj =>
            {
                _running = false;
                UpdateCommand.RaiseCanExecuteChanged();
            });
        }

        public IFoldersView View { get; set; }
        public DelegateCommand<FolderModel> UpdateCommand { get; set; }

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
