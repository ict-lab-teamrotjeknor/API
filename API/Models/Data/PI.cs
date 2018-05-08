using Microsoft.AspNetCore.Identity;

namespace API.Models.Data
{
    public class PI
    {
        public string Id { get; set; }
        public string MacAdress { get; set; }

        public Classroom Classroom { get; set; }
    }
}