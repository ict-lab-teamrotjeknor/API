using System.ComponentModel.DataAnnotations;

namespace API.Models.Data
{
    public class NotificationUser
    {
        [Key]
        public string Id { get; set; }
        public bool New { get; set; }
        public string UserId { get; set; }
        public string NotificationId { get; set; }

        public NotificationMessage notification { get; set; }
        public User user { get; set; }
    }
}