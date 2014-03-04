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

            UpdateAllCommand = new DelegateCommand(
                ()=> _eventAggregator.GetEvent<UpdateAllEvent>().Publish(null),
                ()=> !_running);

            _eventAggregator.GetEvent<ProcessStartEvent>().Subscribe(obj =>
                {
                    _running = true;
                    SelectCommand.RaiseCanExecuteChanged();
                    UpdateAllCommand.RaiseCanExecuteChanged();
                });
            _eventAggregator.GetEvent<ProcessEndEvent>().Subscribe(obj =>
            {
                _running = false;
                SelectCommand.RaiseCanExecuteChanged();
                UpdateAllCommand.RaiseCanExecuteChanged();
            });
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
        public DelegateCommand UpdateAllCommand { get; set; }
    }
}
