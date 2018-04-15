using System.Collections.Generic;
using API.Models.Data;

namespace API.Process.Model
{
    public class Sensors
    {
        public string RoomName { get; set; }
        public List<ModelSensor> AllSensors { get; set; }

        public void AddSensors(List<Sensor> newSensors)
        {
            AllSensors = new List<ModelSensor>();
            foreach (var sensor in newSensors)
            {
                var newModel = new ModelSensor();
                newModel.Id = sensor.Id;
                newModel.Name = sensor.Name;
                newModel.Type = sensor.Type;
                AllSensors.Add(newModel);
            }
        }
    }
}