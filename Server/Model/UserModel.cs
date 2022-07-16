using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class UserModel
    {
        /// <summary>
        /// Login of the user
        /// </summary>
        public string? Login { get; set; }

        /// <summary>
        /// Password of the user
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Total played games
        /// </summary>
        public int TotalGames { get; set; }

        /// <summary>
        /// Total wins
        /// </summary>
        public int TotalWins { get; set; }

        /// <summary>
        /// Earned coins
        /// </summary>
        public int Coins { get; set; }
    }
}
