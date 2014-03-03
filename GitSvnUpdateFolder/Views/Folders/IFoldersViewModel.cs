using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSvnUpdateFolder.Models;
using System.Collections.ObjectModel;

namespace GitSvnUpdateFolder.Views.Folders
{
    public interface IFoldersViewModel
    {
        IFoldersView View { get; set; }

        ObservableCollection<FolderModel> Folders { get; set; }
        FolderModel SelectedFolder { get; set; }
    }
}
