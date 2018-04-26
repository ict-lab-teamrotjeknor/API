using System.Collections.Generic;
using API.Models.Data;

namespace API.Process.Model
{
    public class UserView
    {
        public List<UserModel> Users { get; set; }

        public UserView()
        {
            Users = new List<UserModel>();
        }
    }
}