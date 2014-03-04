using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitSvnUpdateFolder.Enums
{
    public enum FolderState
    {
        Updated,
        Updating,
        Outdated,
        Error,
        Info,
        Initializing
    }
}
