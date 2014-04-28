using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSvnUpdateFolder.Enums;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using GitUIPluginInterfaces;
using GitCommands;

namespace GitSvnUpdateFolder.Models
{
    public class FolderModel : NotificationObject
    {
        public FolderModel(string path)
        {
            Name = new DirectoryInfo(path).Name;
            FullPath = path;
            
            Output = new ObservableCollection<MessageModel>();
            Batches = new ObservableCollection<string>();

            State = FolderState.Outdated;

            GitModule = new GitModule(path);

            //Refresh();

            //State = FolderState.Initializing;
            //Task.Factory.StartNew(() =>
            //    {
            //        var batchs = Directory.GetFiles(path, "*.bat", SearchOption.AllDirectories).ToList();

            //        App.Current.Dispatcher.Invoke(new Action(() =>
            //            {
            //                Batches.Clear();
            //                batchs.ForEach(f => Batches.Add(f));

            //                if (State == FolderState.Initializing)
            //                    State = FolderState.Outdated;
            //            }));
            //    });
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

        public ObservableCollection<MessageModel> Output { get; private set; }
        public ObservableCollection<string> Batches { get; private set; }

        private volatile bool _Stoped = true;
        public bool Stoped
        {
            get
            {
                return _Stoped;
            }
            set
            {
                _Stoped = value;
                RaisePropertyChanged(() => Stoped);
            }
        }

        private int _ChangedCount;
        public int ChangedCount
        {
            get
            {
                return _ChangedCount;
            }
            set
            {
                _ChangedCount = value;
                RaisePropertyChanged(() => ChangedCount);
            }
        }

        private int _PendingCommits;
        public int PendingCommits
        {
            get
            {
                return _PendingCommits;
            }
            set
            {
                _PendingCommits = value;
                RaisePropertyChanged(() => PendingCommits);
            }
        }

        public GitModule GitModule { get; set; }

        public void Refresh()
        {
            if (GitModule.IsValidGitWorkingDir())
            {
                var command = GitCommandHelpers.GetAllChangedFilesCmd(true, UntrackedFilesMode.Default);
                var updatedStatus = GitModule.RunGit(command);

                var allChangedFiles = GitCommandHelpers.GetAllChangedFilesFromString(GitModule, updatedStatus);
                var stagedCount = allChangedFiles.Count(status => status.IsStaged);
                var unstagedCount = allChangedFiles.Count - stagedCount;
                var unstagedSubmodulesCount = allChangedFiles.Count(status => status.IsSubmodule && !status.IsStaged);

                ChangedCount = allChangedFiles.Count;

                var cmd = "log trunk..HEAD --graph --pretty=format:'%Cred%h%Creset' --abbrev-commit --date=relative";
                var result = GitModule.RunGit(cmd);
                var msgs = result.Split('*');
                PendingCommits = msgs.Length - 1;
            }
        }
    }
}
