using System;

namespace API.Process.Model
{
    public class Notification
    {
        public string ID { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool New { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}