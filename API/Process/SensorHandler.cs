using System.Net.Mime;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace API.Process
{
    //This class handles every sensors
    public class SensorHandler
    {
        private IDbSensor _dbSensor;
        private IJsonEditor _jsonEditor;
        private ILogger _logger;
        private bool Deployment;

        public SensorHandler(IDbSensor newDbSensor, ILogger logger, bool newDeployment = true)
        {
            _dbSensor = newDbSensor;
            _jsonEditor = new JsonEditor(logger);
            Deployment = newDeployment;
        }

        public SensorHandler(IDbSensor newDbSensor, IJsonEditor jsonEditor)
        {
            _dbSensor = newDbSensor;
            _jsonEditor = jsonEditor;
            Deployment = false;
        }

        //Add a new sensor to the pi
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
                if (Deployment) _logger.LogInformation("Sensor already exists");
                return _jsonEditor.GetError("Sensor already exists");
            }
        }

        //Add new data to a sensor
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
                if (Deployment) _logger.LogInformation("Sensor doesn't exists");
                return _jsonEditor.GetError("Sensor doesn't exists");
            }
        }

        //Get every sensor of a room
        public JObject GetSensors(string roomName)
        {
            var sensors = _dbSensor.GetSensors(roomName);
            var allSensors = new Sensors();
            allSensors.RoomName = roomName;
            allSensors.AddSensors(sensors);
            return _jsonEditor.SerilizeJObject(allSensors);
        }

        //Get every data of a sensor
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
                if (Deployment) _logger.LogInformation("Sensor doesn't exists");
                return _jsonEditor.GetError("Sensor doesn't exists");
            }
        }
}
}