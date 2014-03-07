using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Events;
using GitSvnUpdateFolder.Events;

namespace GitSvnUpdateFolder.Views.FolderSelector
{
    public class FolderSelectorViewModel : NotificationObject, IFolderSelectorViewModel
    {
        private IEventAggregator _eventAggregator;
        private bool _running = false;

        public FolderSelectorViewModel(IFolderSelectorView view, IEventAggregator eventAggregator)
        {
            View = view;
            View.DataContext = this;

            _eventAggregator = eventAggregator;

            SelectCommand = new DelegateCommand(() =>
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FolderPath = dialog.SelectedPath;
                }
            }, ()=> !_running);

            FetchAllCommand = new DelegateCommand(
                () => _eventAggregator.GetEvent<FetchAllEvent>().Publish(null),
                () => !_running);

            RebaseAllCommand = new DelegateCommand(
                () => _eventAggregator.GetEvent<RebaseAllEvent>().Publish(null),
                () => !_running);

            CommitAllCommand = new DelegateCommand(
                () => _eventAggregator.GetEvent<CommitAllEvent>().Publish(null),
                () => !_running);

            _eventAggregator.GetEvent<ProcessStartEvent>().Subscribe(obj => UpdateCommands(true));
            _eventAggregator.GetEvent<ProcessEndEvent>().Subscribe(obj => UpdateCommands(false));
        }

        private void UpdateCommands(bool running)
        {
            _running = running;
            SelectCommand.RaiseCanExecuteChanged();
            FetchAllCommand.RaiseCanExecuteChanged();
            RebaseAllCommand.RaiseCanExecuteChanged();
            CommitAllCommand.RaiseCanExecuteChanged();
        }

        public IFolderSelectorView View { get; set; }

        private string _FolderPath;
        public string FolderPath
        {
            get
            {
                return _FolderPath;
            }
            set
            {
                _FolderPath = value;
                RaisePropertyChanged(() => FolderPath);
                _eventAggregator.GetEvent<FolderSelectedEvent>().Publish(value);
            }
        }

        public DelegateCommand SelectCommand { get; set; }
        public DelegateCommand FetchAllCommand { get; set; }
        public DelegateCommand RebaseAllCommand { get; set; }
        public DelegateCommand CommitAllCommand { get; set; }
    }
}
