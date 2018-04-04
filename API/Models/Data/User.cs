using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Models.Data
{
    public class User : IdentityUser
    {
        public List<Hour> Hours { get; set; }
    }
}