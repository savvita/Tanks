using Server.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server.ViewModel
{
    public class SessioInfo
    {
        public SessionModel Session { get; set; }
        public int Count { get => Session.Clients.Count; }
    }
    public class ServerViewModel : INotifyPropertyChanged
    {
        public ServerModel Server { get; } = new ServerModel();

        public List<UserModel>? Users
        {
            get => Server.GetAllUsers();
        }

        public List<SessioInfo>? Sessions
        {
            get => Server.GetAllSessions().Select(x => new SessioInfo() { Session = x}).ToList();
        }



        public ServerViewModel()
        {
            Task.Factory.StartNew(Server.Listen);

            Server.UsersChanged += () => { OnPropertyChanged(nameof(Users)); };
            Server.SessionsChanged += () => { OnPropertyChanged(nameof(Sessions)); };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Disconnect()
        {
            Server.Close();
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
