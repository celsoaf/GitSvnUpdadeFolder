﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSvnUpdateFolder.Enums;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace GitSvnUpdateFolder.Models
{
    public class FolderModel : NotificationObject
    {
        public FolderModel(string path)
        {
            Name = new DirectoryInfo(path).Name;
            FullPath = path;
            State = FolderState.Initializing;

            Output = new ObservableCollection<string>();
            Batches = new ObservableCollection<string>();

            Task.Factory.StartNew(() =>
                {
                    var batchs = Directory.GetFiles(path, "*.bat", SearchOption.AllDirectories).ToList();

                    App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            Batches.Clear();
                            batchs.ForEach(f => Batches.Add(f));

                            if (State == FolderState.Initializing)
                                State = FolderState.Outdated;
                        }));
                });
        }

        public string Name { get; private set; }
        public string FullPath { get; private set; }
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

        public ObservableCollection<string> Output { get; private set; }
        public ObservableCollection<string> Batches { get; private set; }
    }
}
