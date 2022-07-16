﻿using Client.Model;
using Connection;

namespace Client.Controller
{
    public class StartController
    {
        public ClientModel? Client { get; }

        public int Coins { get; private set; }

        public int Health { get; private set; }

        public int Damage { get; private set; }

        public StartController(ClientModel client)
        {
            Client = client;

            SetValues();
        }

        public void AcceptNewValues(int health, int damage, int coins)
        {
            SendNewValues(health, damage, coins);
            SetValues();
        }

        private void SetValues()
        {
            if (Client != null)
            {
                bool isSuccess;
                try
                {
                    Coins = int.Parse(Client.ReceiveMessage(out isSuccess));
                    Health = int.Parse(Client.ReceiveMessage(out isSuccess));
                    Damage = int.Parse(Client.ReceiveMessage(out isSuccess));
                }
                catch { }
            }
        }

        private void SendNewValues(int health, int damage, int coins)
        {
            if (Client != null)
            {
                Client.SendMessage(SocketClient.ShopCode);
                Client.SendMessage(health.ToString());
                Client.SendMessage(damage.ToString());
                Client.SendMessage(coins.ToString());
            }
        }
    }
}
