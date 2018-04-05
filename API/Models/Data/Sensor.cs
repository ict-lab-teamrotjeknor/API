using System.Collections.Generic;

namespace API.Models.Data
{
    public class Sensor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string RoomId { get; set; }

        public Classroom Classroom { get; set; }
        public List<SensorData> SensorDatas { get; set; }
    }
}