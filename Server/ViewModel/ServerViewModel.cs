using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ViewModel
{
    public class ServerViewModel
    {
        public ServerModel Server { get; } = new ServerModel();
        public ServerViewModel()
        {
            Task.Factory.StartNew(Server.Listen);
        }
    }
}
