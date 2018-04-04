using System.Collections.Generic;

namespace API.Models.Data
{
    public class Classroom
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BuildingId { get; set; }

        public List<Year> Years { get; set; }
    }
}