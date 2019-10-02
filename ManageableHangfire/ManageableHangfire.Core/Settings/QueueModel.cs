using System;
using System.Collections.Generic;
using System.Text;

namespace ManageableHangfire.Core.Settings
{
    public class QueueModel
    {
        public string Name { get; set; }
        public int Workers { get; set; }
    }
}
