﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace GitSvnUpdateFolder.Views.FolderSelector
{
    public interface IFolderSelectorViewModel
    {
        IFolderSelectorView View { get; set; }

        string FolderPath { get; set; }
        DelegateCommand SelectCommand { get; set; }
        DelegateCommand FetchAllCommand { get; set; }
        DelegateCommand RebaseAllCommand { get; set; }
        DelegateCommand CommitAllCommand { get; set; }
        DelegateCommand BrowseCommand { get; set; }
    }
}
