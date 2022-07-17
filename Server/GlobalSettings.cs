using System;
using System.IO;

namespace Server
{
    public static class GlobalSettings
    {
        /// <summary>
        /// File to keep registered users
        /// </summary>
        public static string UsersFile { get; } = Path.Combine(Environment.CurrentDirectory, "users.txt");

        /// <summary>
        /// Coins to the winner
        /// </summary>
        public static int Coins { get; } = 100;

        /// <summary>
        /// Default health of the tank
        /// </summary>
        public static int Health { get; } = 100;

        /// <summary>
        /// Default damage of the tank
        /// </summary>
        public static int Damage { get; } = 50;

        /// <summary>
        /// Maximum players per sesson
        /// </summary>
        public static int MaxPlayers { get; } = 4;
    }
}
