using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitSvnUpdateFolder.Views.Output
{
    public interface IOutputView
    {
        object DataContext { get; set; }
    }
}
