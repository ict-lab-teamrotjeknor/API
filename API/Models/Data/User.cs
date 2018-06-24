using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Models.Data
{
    public class User : IdentityUser
    {
        public List<Hour> Hours { get; set; }
        public List<NotificationUser> notfications { get; set; }

        public bool Delete { get; set; }

        public User()
        {
            Delete = false;
        }
    }
}