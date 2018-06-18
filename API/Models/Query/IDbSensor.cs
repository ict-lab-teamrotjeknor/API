using System;
using System.Collections.Generic;
using API.Process.Model;

namespace API.Models.Data.Query
{
    public interface IDbSensor
    {
        bool SaveSensor(Sensor newSensor, string roomName);
        Sensor GetSensor(string sensorName, string roomName);
        List<Sensor> GetSensors(string roomName);
        bool AddData(NewSensorData data);
        List<SensorData> GetData(string sensorName, string roomName);
    }
}