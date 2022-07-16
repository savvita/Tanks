﻿using Client.Model;

namespace Client.Controller
{
    public class AuthorizationController
    {
        public ClientModel? Client { get; } = new ClientModel();


        /// <summary>
        /// Athorization at the server
        /// </summary>
        /// <param name="login">Login of the client</param>
        /// <param name="password">Password of the client</param>
        /// <returns>True if athorization is successful otherwise false</returns>
        public bool Login(string login, string password)
        {
            if(Client == null)
            {
                return false;
            }

            bool result = Client.Authorizate(login, password);

            if(result)
            {
                Client.Name = login;
            }

            return result;
        }

        /// <summary>
        /// Register a new client
        /// </summary>
        /// <param name="login">Login of the client</param>
        /// <param name="password">Password of the client</param>
        /// <returns>True if registration is successful otherwise false</returns>
        public bool Register(string login, string password)
        {
            if (Client == null)
            {
                return false;
            }

            bool result = Client.Register(login, password);

            if(result)
            {
                Client.Name = login;
            }

            return result;
        }
    }
}
