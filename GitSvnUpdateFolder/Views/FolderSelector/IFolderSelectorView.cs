using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitSvnUpdateFolder.Views.FolderSelector
{
    public interface IFolderSelectorView
    {
        object DataContext { get; set; }
    }
}
