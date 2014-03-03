using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace GitSvnUpdateFolder.Views.FolderSelector
{
    public interface IFolderSelectorViewModel
    {
        IFolderSelectorView View { get; set; }

        string FolderPath { get; set; }
        ICommand SelectCommand { get; set; }
    }
}
