using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitSvnUpdateFolder.Views.Batches
{
    public interface IBatchesView
    {
        object DataContext { get; set; }
    }
}
