using System.Collections.Generic;

namespace API.Models.Data
{
    public class Year
    {
        public string Id { get; set; }
        public int SchoolYear { get; set; }
        public string RoomId { get; set; }
        
        public List<Period> Periods { get; set; }
        public Classroom Classroom { get; set; }
    }
}