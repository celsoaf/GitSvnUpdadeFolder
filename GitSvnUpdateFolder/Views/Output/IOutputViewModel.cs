using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using GitSvnUpdateFolder.Models;

namespace GitSvnUpdateFolder.Views.Output
{
    public interface IOutputViewModel
    {
        IOutputView View { get; set; }

        ObservableCollection<MessageModel> Output { get; set; }

        string Header { get; set; }
    }
}
