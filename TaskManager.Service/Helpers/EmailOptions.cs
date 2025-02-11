using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Service.Helpers
{
    public class EmailOptions
    {
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string AppPassword { get; set; }
        public int PortSSL { get; set; }
        public int Port { get; set; }

    }
}
