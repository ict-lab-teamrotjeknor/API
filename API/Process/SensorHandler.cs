using System.Net.Mime;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process.Model;
using Newtonsoft.Json.Linq;

namespace API.Process
{
    public class SensorHandler
    {
        private DbSensor _dbSensor;
        private JsonEditor _jsonEditor;

        public SensorHandler(ApplicationDbContext newDbContext)
        {
            _dbSensor = new DbSensor(newDbContext);
            _jsonEditor = new JsonEditor();
        }

        public JObject AddSensor(JObject sensor)
        {
            var newsensor = _jsonEditor.GetSensor(sensor);
            var room = _jsonEditor.GetRoom(sensor);
            var sensorExists = _dbSensor.GetSensor(newsensor.Name, room);

            if (sensorExists == null)
            {
                _dbSensor.SaveSensor(newsensor, room);
                return _jsonEditor.GetSucced();
            }
            else
            {
                return _jsonEditor.GetError("Sensor already exists");
            }
        }

        public JObject AddData(NewSensorData data)
        {
            var sensor = _dbSensor.GetSensor(data.Name, data.Room);
            if (sensor != null)
            {
                var done = _dbSensor.AddData(data);
                return _jsonEditor.GetSucced();
            }
            else
            {
                return _jsonEditor.GetError("Sensor doesn't exists");
            }
        }

        public JObject GetSensors(string roomName)
        {
            var sensors = _dbSensor.GetSensors(roomName);
            var allSensors = new Sensors();
            allSensors.RoomName = roomName;
            allSensors.AddSensors(sensors);
            return _jsonEditor.SerilizeJObject(allSensors);
        }

        public JObject GetData(string roomName, string sensorName)
        {
            var sensor = _dbSensor.GetSensor(sensorName, roomName);

            if (sensor != null)
            {
                var data = _dbSensor.GetData(sensorName, roomName);

                var allData = new ModelSensorData();
                allData.Name = sensorName;
                allData.Type = sensor.Type;
                allData.SetData(data);

                return _jsonEditor.SerilizeJObject(allData);
            }
            else
            {
                return _jsonEditor.GetError("Sensor doesn't exists");
            }
        }
}
}