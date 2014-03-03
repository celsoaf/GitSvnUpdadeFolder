using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace GitSvnUpdateFolder.Views.Output
{
    public interface IOutputViewModel
    {
        IOutputView View { get; set; }

        ObservableCollection<string> Output { get; set; }
    }
}
