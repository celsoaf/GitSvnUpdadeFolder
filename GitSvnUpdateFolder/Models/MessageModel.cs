using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSvnUpdateFolder.Enums;

namespace GitSvnUpdateFolder.Models
{
    public class MessageModel
    {
        public string Message { get; set; }
        public MessageType Type { get; set; }
    }
}
