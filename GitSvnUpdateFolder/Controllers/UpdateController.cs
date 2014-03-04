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
               RegionNames.TopRegion,
               () => _folderSelector.View);

            _regionManager.RegisterViewWithRegion(
                RegionNames.RightRegion,
                () => _output.View);

            InitializeEvents();

            _folderSelector.FolderPath = DEFAULT_FOLDER;
        }

        private void InitializeEvents()
        {
            _eventAggregator.GetEvent<FolderSelectedEvent>().Subscribe(FolderSelected);
            _eventAggregator.GetEvent<UpdateFolderEvent>().Subscribe(f =>
            {
                _eventAggregator.GetEvent<ProcessStartEvent>().Publish(null);

                Task.Factory.StartNew(() =>
                {
                    Update(f);

                    _eventAggregator.GetEvent<ProcessEndEvent>().Publish(null);
                });
            });
            _eventAggregator.GetEvent<UpdateAllEvent>().Subscribe(obj =>
            {
                StartUpdate();
            });
        }

        private void FolderSelected(string path)
        {
            _folder.Folders.Clear();

            foreach (var folder in Directory.GetDirectories(path).OrderBy(s => s))
            {
                _folder.Folders.Add(new Models.FolderModel
                {
                    Name = new DirectoryInfo(folder).Name,
                    FullPath = folder,
                    State = Enums.FolderState.Outdated
                });
            }
        }

        private void StartUpdate()
        {
            _eventAggregator.GetEvent<ProcessStartEvent>().Publish(null);
            _eventAggregator.GetEvent<ProcessProgressEvent>().Publish(0);

            Task.Factory.StartNew(() =>
                {
                    var list = _folder.Folders.ToList();

                    var total = list.Count;
                    var i = 0d;
                    foreach (var item in list)
                    {
                        Update(item);

                        var progress = (++i / total) * 100;
                        _eventAggregator.GetEvent<ProcessProgressEvent>().Publish(progress);
                    }

                    _eventAggregator.GetEvent<ProcessEndEvent>().Publish(null);
                });
        }

        private void Update(FolderModel folder)
        {
            folder.State = Enums.FolderState.Updating;

            //git svn fetch
            Directory.SetCurrentDirectory(folder.FullPath);

            var p = new Process();
            p.StartInfo = new ProcessStartInfo("git", "svn fetch")
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
                        folder.Output.Add(e.Data);
                    }));
                }
            };

            p.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null)
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        folder.Output.Add(e.Data);
                    }));
                }
            };

            p.EnableRaisingEvents = true;

            p.Start();

            p.BeginErrorReadLine();

            p.BeginOutputReadLine();

            p.WaitForExit();

            if (p.ExitCode == 0)
                folder.State = folder.Output.Any() ? Enums.FolderState.Info : Enums.FolderState.Updated;
            else
                folder.State = Enums.FolderState.Error;
        }
    }
}
