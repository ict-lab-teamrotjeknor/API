using System;
using System.Collections.Generic;
using System.Globalization;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process.Model;

namespace UnitTest
{
    public class DbSensorFake : IDbSensor
    {
        public DbSensorFake()
        {
            
        }
        
        public bool SaveSensor(Sensor newSensor, string roomName)
        {
            return true;
        }

        public Sensor GetSensor(string sensorName, string roomName)
        {
            var sensor = new Sensor();
            sensor.Name = "SensorTest";
            sensor.Type = "Nothing";
            return roomName.Equals("Name") || roomName.Equals("TestRoom") ? sensor : null;
        }

        public List<Sensor> GetSensors(string roomName)
        {
            var sensors = new List<Sensor>();
            var newSensor = new Sensor();
            newSensor.Type = "Nothing";
            newSensor.Id = "1";
            newSensor.Name = "Test";
            sensors.Add(newSensor);
            return sensors;
        }

        public bool AddData(NewSensorData data)
        {
            return true;
        }

        public List<SensorData> GetData(string sensorName, string roomName)
        {
            var datas = new List<SensorData>();
            var data = new SensorData();
            data.Value = "50";
            data.Date = DateTime.ParseExact("2018-10-10 10:40:00,000", "yyyy-MM-dd HH:mm:ss,fff",CultureInfo.InvariantCulture);
            datas.Add(data);
            return datas;
        }
    }
}