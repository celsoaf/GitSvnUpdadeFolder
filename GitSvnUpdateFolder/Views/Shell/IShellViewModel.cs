using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;

namespace GitSvnUpdateFolder.Views.Shell
{
    public interface IShellViewModel
    {
        IShellView View { get; set; }

        double ProgressValue { get; set; }

        DelegateCommand FetchAllCommand { get; set; }
        DelegateCommand RebaseAllCommand { get; set; }
        DelegateCommand CommitAllCommand { get; set; }
        DelegateCommand BrowseCommand { get; set; }
    }
}
