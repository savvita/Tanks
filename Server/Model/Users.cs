using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Server.Model
{
    public class Users
    {
        private List<UserModel>? users = new List<UserModel>();

        public Users()
        {
            LoadUsers();
        }

        /// <summary>
        /// Load the list of the users
        /// </summary>
        public void LoadUsers()
        {
            if (File.Exists(GlobalSettings.UsersFile))
            {
                try
                {
                    string _users = File.ReadAllText(GlobalSettings.UsersFile);
                    users = JsonSerializer.Deserialize<List<UserModel>>(_users);
                }
                catch { }
            }
        }

        /// <summary>
        /// Save the list of the users
        /// </summary>
        public void SaveUsers()
        {
            if (!File.Exists(GlobalSettings.UsersFile))
            {
                File.Create(GlobalSettings.UsersFile);
            }

            try
            {
                if (users != null)
                {
                    string _users = JsonSerializer.Serialize<List<UserModel>>(users);
                    File.WriteAllText(GlobalSettings.UsersFile, _users);
                }
            }
            catch { }
        }


        /// <summary>
        /// Check if the user is registered
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        /// <returns>True if the user is registered otherwise false</returns>
        public bool IsUserRegistered(string login, string password)
        {
            if (users == null)
            {
                return false;
            }

            return users.Any(x => x.Login == login && x.Password == password);
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="login">Login of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>Usermodel if registration is successful otherwise null</returns>
        public UserModel? RegisterUser(string login, string password)
        {
            if (users == null)
            {
                return null;
            }

            if (users.Any(x => x.Login != null && x.Login.Equals(login)))
            {
                return null;
            }

            return AddToTheList(login, password);
        }

        /// <summary>
        /// Add a new user to the liset of registred users
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>Model of a new user</returns>
        private UserModel AddToTheList(string? login, string? password)
        {
            UserModel model = new UserModel()
            {
                Login = login,
                Password = password,
            };

            users?.Add(model);

            return model;
        }

        /// <summary>
        /// Increase the count of wins and add the coins to the winner
        /// </summary>
        /// <param name="name">Name of the winner</param>
        public void SetWinner(string? name)
        {
            UserModel? user = GetUserByName(name);

            if (user != null)
            {
                user.TotalWins++;
                user.Coins += GlobalSettings.Coins;
            }
        }

        /// <summary>
        /// Increase a total count of games when user starts a new game
        /// </summary>
        /// <param name="name">Name of the user</param>
        public void SetTotalGames(string? name)
        {
            UserModel? user = GetUserByName(name);

            if (user != null)
            {
                user.TotalGames++;
            }
        }

        /// <summary>
        /// Get coins of the user
        /// </summary>
        /// <param name="name">Name of the user</param>
        /// <returns>Coins</returns>
        public int GetCoins(string? name)
        {
            UserModel? user = GetUserByName(name);

            if (user != null)
            {
                return user.Coins;
            }

            return 0;
        }

        /// <summary>
        /// Find user by name
        /// </summary>
        /// <param name="name">Name of the user</param>
        /// <returns>Model of the user or null if the user is not found</returns>
        public UserModel? GetUserByName(string? name)
        {
            if (users == null || name == null)
            {
                return null;
            }

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Login == null)
                {
                    continue;
                }

                if (users[i].Login!.Equals(name))
                {
                    return users[i];
                }
            }

            return null;
        }
    }
}
