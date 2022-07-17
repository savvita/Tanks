using Server.Model;

namespace Server.ViewModel
{
    public class SessionInfo
    {
        /// <summary>
        /// Game session
        /// </summary>
        public SessionModel? Session { get; set; }

        /// <summary>
        /// Count players at the session
        /// </summary>
        public int Count
        {
            get
            {
                if (Session != null && Session.Clients != null)
                    return Session.Clients.Count;

                return 0;
            }
        }
    }
}
