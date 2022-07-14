using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            if (File.Exists(usersFile))
            {
                try
                {
                    string _users = File.ReadAllText(usersFile);
                    users = JsonSerializer.Deserialize<List<UserModel>>(_users);
                }
                catch { }
            }
        }

        private string usersFile = Path.Combine(Environment.CurrentDirectory, "users.txt");

        /// <summary>
        /// Save the list of the users
        /// </summary>
        public void SaveUsers()
        {
            if (!File.Exists(usersFile))
            {
                File.Create(usersFile);
            }

            try
            {
                if (users != null)
                {
                    string _users = JsonSerializer.Serialize<List<UserModel>>(users);
                    File.WriteAllText(usersFile, _users);
                }
            }
            catch { }
        }

        public void SetWin(string name)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Login.Equals(name))
                {
                    users[i].TotalWins++;
                    users[i].Score += 100;
                    break;
                }
            }
        }

        public void SetTotal(string name)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Login.Equals(name))
                {
                    users[i].TotalGames++;
                    break;
                }
            }
        }

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
        /// Check if the user is registered
        /// </summary>
        /// <param name="ip">IP of the user</param>
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
        /// <param name="ip">IP of the user</param>
        /// <param name="login">Login of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>True if a new user is registred otherwise false</returns>
        public bool RegisterUser(string login, string password)
        {
            if (users == null)
            {
                return false;
            }

            if (users.Any(x => x.Login != null && x.Login.Equals(login)))
            {
                return false;
            }

            AddToTheList(login, password);
            return true;
        }

    }
}
