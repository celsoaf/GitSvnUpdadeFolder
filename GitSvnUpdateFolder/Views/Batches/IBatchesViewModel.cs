using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace GitSvnUpdateFolder.Views.Batches
{
    public interface IBatchesViewModel
    {
        IBatchesView View { get; set; }

        ObservableCollection<string> List { get; set; }
    }
}
