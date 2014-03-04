using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitSvnUpdateFolder.Views.Progress
{
    public interface IProgressView
    {
        object DataContext { get; set; }
    }
}
