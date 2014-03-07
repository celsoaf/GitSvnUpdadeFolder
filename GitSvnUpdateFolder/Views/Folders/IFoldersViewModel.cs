using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSvnUpdateFolder.Models;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;

namespace GitSvnUpdateFolder.Views.Folders
{
    public interface IFoldersViewModel
    {
        IFoldersView View { get; set; }

        ObservableCollection<FolderModel> Folders { get; set; }
        FolderModel SelectedFolder { get; set; }
        DelegateCommand<FolderModel> FetchCommand { get; set; }
        DelegateCommand<FolderModel> RebaseCommand { get; set; }
        DelegateCommand<FolderModel> CommitCommand { get; set; }
    }
}
