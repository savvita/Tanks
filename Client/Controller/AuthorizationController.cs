using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controller
{
    public class AuthorizationController
    {
        public ClientModel? Client { get; }

        public AuthorizationController()
        {
            Client = new ClientModel("127.0.0.1", 8008);
        }

        public bool Login(string login, string password)
        {
            if(Client == null)
            {
                return false;
            }

            return Client.Authorizate(login, password);
        }

        public bool Register(string login, string password)
        {
            if (Client == null)
            {
                return false;
            }

            return Client.Register(login, password);
        }
    }
}
