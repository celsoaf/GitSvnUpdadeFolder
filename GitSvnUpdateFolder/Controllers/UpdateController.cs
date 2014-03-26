using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using GitSvnUpdateFolder.Views.Folders;
using GitSvnUpdateFolder.Views.FolderSelector;
using GitSvnUpdateFolder.Events;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using GitSvnUpdateFolder.Models;
using GitSvnUpdateFolder.Views.Output;
using GitSvnUpdateFolder.Views.Progress;
using GitSvnUpdateFolder.Views.Batches;

namespace GitSvnUpdateFolder.Controllers
{
    public class UpdateController : IUpdateController
    {
        private const string DEFAULT_FOLDER = @"C:\GitSvn";

        private IUnityContainer _container;
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;

        private IFoldersViewModel _folder;
        private IFolderSelectorViewModel _folderSelector;
        private IOutputViewModel _output;

        public UpdateController(
            IUnityContainer container,
            IEventAggregator eventAggregator,
            IRegionManager regionManager)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            _folder = _container.Resolve<IFoldersViewModel>();
            _folderSelector = _container.Resolve<IFolderSelectorViewModel>();
            _output = _container.Resolve<IOutputViewModel>();

            _regionManager.RegisterViewWithRegion(
                RegionNames.LeftRegion,
                () => _folder.View);

            _regionManager.RegisterViewWithRegion(
               RegionNames.Top1Region,
               () => _folderSelector.View);

            _regionManager.RegisterViewWithRegion(
                RegionNames.RightBottomRegion,
                () => _output.View);

            _regionManager.RegisterViewWithRegion(
                RegionNames.Top2Region,
                () => _container.Resolve<IProgressViewModel>().View);

            _regionManager.RegisterViewWithRegion(
                RegionNames.RightTopRegion,
                () => _container.Resolve<IBatchesViewModel>().View);

            InitializeEvents();

            _folderSelector.FolderPath = DEFAULT_FOLDER;
        }

        private void InitializeEvents()
        {
            _eventAggregator.GetEvent<FolderSelectedEvent>().Subscribe(FolderSelected);
            _eventAggregator.GetEvent<FetchFolderEvent>().Subscribe(f =>
            {
                _eventAggregator.GetEvent<ProcessStartEvent>().Publish(null);

                Task.Factory.StartNew(() =>
                {
                    FetchFolder(f);

                    _eventAggregator.GetEvent<ProcessEndEvent>().Publish(null);
                });
            });
            _eventAggregator.GetEvent<RebaseFolderEvent>().Subscribe(f =>
            {
                _eventAggregator.GetEvent<ProcessStartEvent>().Publish(null);

                Task.Factory.StartNew(() =>
                {
                    RebaseFolder(f);

                    _eventAggregator.GetEvent<ProcessEndEvent>().Publish(null);
                });
            });
            _eventAggregator.GetEvent<CommitFolderEvent>().Subscribe(f =>
            {
                _eventAggregator.GetEvent<ProcessStartEvent>().Publish(null);

                Task.Factory.StartNew(() =>
                {
                    CommitFolder(f);

                    _eventAggregator.GetEvent<ProcessEndEvent>().Publish(null);
                });
            });
            _eventAggregator.GetEvent<FetchAllEvent>().Subscribe(obj =>
            {
                RunAll(FetchFolder);
            });
            _eventAggregator.GetEvent<RebaseAllEvent>().Subscribe(obj =>
            {
                RunAll(RebaseFolder);
            });
            _eventAggregator.GetEvent<CommitAllEvent>().Subscribe(obj =>
            {
                RunAll(CommitFolder);
            });


            _eventAggregator.GetEvent<GitExtentionsEvent>().Subscribe(f =>
            {
                RunGitExtensions(f);
            });
            _eventAggregator.GetEvent<BrowseEvent>().Subscribe(path =>
            {
                OpenFolder(path);
            });
        }

        private void FolderSelected(string path)
        {
            _eventAggregator.GetEvent<ProcessStartEvent>().Publish(null);

            _folder.Folders.Clear();

            Task.Factory.StartNew(() =>
                {
                    foreach (var folder in Directory.GetDirectories(path).OrderBy(s => s))
                    {
                        var fm = new Models.FolderModel(folder);

                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            _folder.Folders.Add(fm);
                        }));
                    }

                    _eventAggregator.GetEvent<ProcessEndEvent>().Publish(null);
                });
        }

        private void RunAll(Action<FolderModel> action)
        {
            _eventAggregator.GetEvent<ProcessStartEvent>().Publish(null);
            _eventAggregator.GetEvent<ProcessProgressEvent>().Publish(0);

            Task.Factory.StartNew(() =>
            {
                var list = _folder.Folders.ToList();

                var total = list.Count;
                var i = 0d;
                var taskList = new List<Task>();
                foreach (var item in list)
                {
                    taskList.Add(Task.Factory.StartNew(() =>
                    {
                        action(item);

                        App.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                var progress = (++i / total) * 100;
                                _eventAggregator.GetEvent<ProcessProgressEvent>().Publish(progress);
                            }));
                    }));
                }

                Task.Factory.ContinueWhenAll(taskList.ToArray(), tl =>
                    {
                        _eventAggregator.GetEvent<ProcessEndEvent>().Publish(null);
                    });
            });
        }

        private void RebaseFolder(FolderModel folder)
        {
            //var gitCmd = new GitUI.GitUICommands(folder.FullPath);

            //var currentBranch = gitCmd.GitCommand("rev-parse --abbrev-ref HEAD").Trim();

            RunGit(folder, "svn rebase");
        }

        private void FetchFolder(FolderModel folder)
        {
            RunGit(folder, "svn fetch");
        }

        private void CommitFolder(FolderModel folder)
        {
            RunGit(folder, "svn dcommit");
        }

        private void RunGitExtensions(FolderModel folder)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(
                @"C:\Program Files (x86)\GitExtensions\GitExtensions.exe",
                "browse " + folder.FullPath)
            {
                UseShellExecute = false,
                //RedirectStandardOutput = true,
                //RedirectStandardError = true,
                CreateNoWindow = true
            };

            p.Start();
        }

        private void OpenFolder(string path)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(
                "explorer", path)
            {
                UseShellExecute = false,
                //RedirectStandardOutput = true,
                //RedirectStandardError = true,
                CreateNoWindow = true
            };

            p.Start();
        }

        private static void RunGit(FolderModel folder, string arguments)
        {
            if (!folder.Stoped)
                return;

            folder.Stoped = false;
            folder.State = Enums.FolderState.Updating;

            App.Current.Dispatcher.Invoke(new Action(() => folder.Output.Clear()));

            //git svn fetch
            Directory.SetCurrentDirectory(folder.FullPath);

            var p = new Process();
            p.StartInfo = new ProcessStartInfo("git", arguments)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            p.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null)
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        folder.Output.Add(new MessageModel { Message = e.Data, Type = Enums.MessageType.Info });
                    }));
                }
            };

            p.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null)
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        folder.Output.Add(new MessageModel { Message = e.Data, Type = Enums.MessageType.Error });
                    }));
                }
            };

            p.EnableRaisingEvents = true;

            p.Start();

            p.BeginErrorReadLine();

            p.BeginOutputReadLine();

            p.WaitForExit();

            if (p.ExitCode != 0 || folder.Output.Any(m => m.Type == Enums.MessageType.Error))
                folder.State = Enums.FolderState.Error;
            else if (folder.Output.Count > 0 && !folder.Output.First().Message.Contains("is up to date."))
                folder.State = Enums.FolderState.Info;
            else
                folder.State = Enums.FolderState.Updated;

            folder.Stoped = true;
        }
    }
}
