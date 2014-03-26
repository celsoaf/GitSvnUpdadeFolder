using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSvnUpdateFolder.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;

namespace GitSvnUpdateFolder.Views.Shell
{
    public class ShellViewModel : NotificationObject, IShellViewModel
    {
        private IEventAggregator _eventAggregator;
        private bool _running = false;
        private string _currentPath;

        public ShellViewModel(IShellView view, IEventAggregator eventAggregator)
        {
            View = view;
            View.DataContext = this;

            _eventAggregator = eventAggregator;

            eventAggregator.GetEvent<ProcessProgressEvent>().Subscribe(p => ProgressValue = p / 100);

            FetchAllCommand = new DelegateCommand(
                () => _eventAggregator.GetEvent<FetchAllEvent>().Publish(null),
                () => !_running);

            RebaseAllCommand = new DelegateCommand(
                () => _eventAggregator.GetEvent<RebaseAllEvent>().Publish(null),
                () => !_running);

            CommitAllCommand = new DelegateCommand(
                () => _eventAggregator.GetEvent<CommitAllEvent>().Publish(null),
                () => !_running);

            BrowseCommand = new DelegateCommand(
                () => _eventAggregator.GetEvent<BrowseEvent>().Publish(_currentPath));

            _eventAggregator.GetEvent<ProcessStartEvent>().Subscribe(obj => UpdateCommands(true));
            _eventAggregator.GetEvent<ProcessEndEvent>().Subscribe(obj => UpdateCommands(false));

            _eventAggregator.GetEvent<FolderSelectedEvent>().Subscribe(path => _currentPath = path);
        }

        private void UpdateCommands(bool running)
        {
            _running = running;
            FetchAllCommand.RaiseCanExecuteChanged();
            RebaseAllCommand.RaiseCanExecuteChanged();
            CommitAllCommand.RaiseCanExecuteChanged();
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

        public DelegateCommand FetchAllCommand { get; set; }
        public DelegateCommand RebaseAllCommand { get; set; }
        public DelegateCommand CommitAllCommand { get; set; }
        public DelegateCommand BrowseCommand { get; set; }
    }
}
