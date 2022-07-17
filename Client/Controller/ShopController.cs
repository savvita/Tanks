using Client.Model;
using Connection;

namespace Client.Controller
{
    public class ShopController
    {
        /// <summary>
        /// Client model of the user
        /// </summary>
        public ClientModel Client { get; }

        /// <summary>
        /// Available coins
        /// </summary>
        public int Coins { get; private set; }

        /// <summary>
        /// Health of the tank
        /// </summary>
        public int Health { get; private set; }

        /// <summary>
        /// Damage of the tank
        /// </summary>
        public int Damage { get; private set; }

        public ShopController(ClientModel client)
        {
            Client = client;

            InitializeShop();

            SetValues();
        }

        /// <summary>
        /// Initialze shop at the server side
        /// </summary>
        private void InitializeShop()
        {
            Client.SendMessage(SocketClient.ShopCode);
            Client.SendMessage(0.ToString());
            Client.SendMessage(0.ToString());
            Client.SendMessage(0.ToString());
        }

        /// <summary>
        /// Accept new values at the server
        /// </summary>
        /// <param name="health">Increment of health</param>
        /// <param name="damage">Increment of damage</param>
        /// <param name="coins">Spent coins</param>
        public void AcceptNewValues(int health, int damage, int coins)
        {
            SendNewValues(health, damage, coins);
            SetValues();
        }

        /// <summary>
        /// Set values to the form
        /// </summary>
        private void SetValues()
        {
            if (Client != null)
            {
                try
                {
                    Coins = int.Parse(Client.ReceiveMessage(out bool _));
                    Health = int.Parse(Client.ReceiveMessage(out bool _));
                    Damage = int.Parse(Client.ReceiveMessage(out bool _));
                }
                catch { }
            }
        }

        /// <summary>
        /// Send new values to the server
        /// </summary>
        /// <param name="health">Increment of health</param>
        /// <param name="damage">Increment of damage</param>
        /// <param name="coins">Spent coins</param>
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
