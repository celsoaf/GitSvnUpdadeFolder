using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitSvnUpdateFolder.Views.Progress
{
    public interface IProgressViewModel
    {
        IProgressView View { get; set; }

        double Value { get; set; }
    }
}
