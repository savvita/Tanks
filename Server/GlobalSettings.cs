using System;
using System.IO;

namespace Server
{
    public static class GlobalSettings
    {
        public static string UsersFile { get; } = Path.Combine(Environment.CurrentDirectory, "users.txt");

        public static int Coins { get; } = 100;

        public static int Health { get; } = 100;

        public static int Damage { get; } = 50;

        public static int MaxPlayers { get; } = 4;
    }
}
