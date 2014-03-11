using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitSvnUpdateFolder.Views.Shell
{
    public interface IShellViewModel
    {
        IShellView View { get; set; }

        double ProgressValue { get; set; }
    }
}
