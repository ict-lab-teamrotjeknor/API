using System.Collections.Generic;
using API.Models.Data;

namespace API.Process.Model
{
    public class ModelSensorData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<SensorDataStructure> Data { get; set; }

        public void SetData(List<SensorData> allData)
        {
            Data = new List<SensorDataStructure>();
            foreach (var data in allData)
            {
                var newData = new SensorDataStructure();
                newData.Value = data.Value;
                newData.Date = data.Date;
                Data.Add(newData);
            }
        }
    }
}