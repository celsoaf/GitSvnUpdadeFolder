using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using GitSvnUpdateFolder.Models;

namespace GitSvnUpdateFolder.Events
{
    public class FolderItemSelectedEvent : CompositePresentationEvent<FolderModel>
    {
    }
}
