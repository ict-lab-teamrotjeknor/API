using System.Collections.Generic;

namespace API.Process.Model
{
    public class Notifications
    {
        public List<Notification> Messages { get; }

        public Notifications()
        {
            Messages = new List<Notification>();
        }
    }
}