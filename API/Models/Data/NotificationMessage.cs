using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Data
{
    public class NotificationMessage
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public IEnumerable<NotificationUser> Notfication { get; set; }
    }
}