using Server.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Server.ViewModel
{
    public class ServerViewModel : INotifyPropertyChanged
    {
        private ServerModel server;

        /// <summary>
        /// Registered users
        /// </summary>
        public List<UserModel>? Users
        {
            get => server.GetAllUsers();
        }

        /// <summary>
        /// Sessions at the server
        /// </summary>
        public List<SessionInfo>? Sessions
        {
            get
            {
                List<SessionModel>? sessions = server.GetAllSessions();
                if (sessions != null)
                {
                    return sessions.Select(x => new SessionInfo() { Session = x }).ToList();
                }

                return null;
            }
        }

        public ServerViewModel()
        {
            server = new ServerModel();
            Task.Factory.StartNew(server.Listen);

            server.UsersChanged += () => { OnPropertyChanged(nameof(Users)); };
            server.SessionsChanged += () => { OnPropertyChanged(nameof(Sessions)); };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Disconnect()
        {
            server.Close();
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
