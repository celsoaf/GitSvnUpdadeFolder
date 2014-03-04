using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using GitSvnUpdateFolder.Views.Shell;
using Microsoft.Practices.Unity;
using GitSvnUpdateFolder.Views.Folders;
using GitSvnUpdateFolder.Controllers;
using GitSvnUpdateFolder.Views.FolderSelector;
using GitSvnUpdateFolder.Views.Output;
using GitSvnUpdateFolder.Views.Progress;

namespace GitSvnUpdateFolder
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<IShellViewModel>().View as DependencyObject;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = Shell as Window;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            registerTypes();

            base.ConfigureContainer();
        }

        private void registerTypes()
        {
            Container.RegisterType<IUpdateController, UpdateController>(
                new ContainerControlledLifetimeManager());

            Container.RegisterType<IShellView, ShellView>();
            Container.RegisterType<IShellViewModel, ShellViewModel>();

            Container.RegisterType<IFoldersView, FoldersView>();
            Container.RegisterType<IFoldersViewModel, FoldersViewModel>();

            Container.RegisterType<IFolderSelectorView, FolderSelectorView>();
            Container.RegisterType<IFolderSelectorViewModel, FolderSelectorViewModel>();

            Container.RegisterType<IOutputView, OutputView>();
            Container.RegisterType<IOutputViewModel, OutputViewModel>();

            Container.RegisterType<IProgressView, ProgressView>();
            Container.RegisterType<IProgressViewModel, ProgressViewModel>();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            Container.Resolve<IUpdateController>();
        }
    }
}
