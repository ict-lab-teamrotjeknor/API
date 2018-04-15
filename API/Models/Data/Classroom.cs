using System.Collections.Generic;

namespace API.Models.Data
{
    public class Classroom
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BuildingId { get; set; }
        public string PiID { get; set; }

        public List<Year> Years { get; set; }
        public List<Sensor> Sensors { get; set; }
        public PI Pi { get; set; }
    }
}