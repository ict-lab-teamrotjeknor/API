using System;
using SQLitePCL;

namespace API.Models.Data
{
    public class SensorData
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
        public string SensorId { get; set; }

        public Sensor Sensor { get; set; }
    }
}