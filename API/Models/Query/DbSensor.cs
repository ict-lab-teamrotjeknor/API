using System;
using System.Collections.Generic;
using System.Linq;
using API.Process.Model;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace API.Models.Data.Query
{
    //Database class for all sensors managing
    public class DbSensor : IDbSensor
    {
        private ApplicationDbContext _dbContext;
        private ILogger _logger;

        public DbSensor(ApplicationDbContext newDbContext, ILogger logger)
        {
            _dbContext = newDbContext;
            _logger = logger;
        }

        public bool SaveSensor(Sensor newSensor, string roomName)
        {
            try
            {
                var room = _dbContext.Classrooms
                    .SingleOrDefault(c => c.Name.Equals(roomName));

                newSensor.RoomId = room.Id;
                
                _dbContext.Add(newSensor);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }

            return true;
        }

        public Sensor GetSensor(string sensorName, string roomName)
        {
            var sensor = _dbContext.Classrooms
                .Join(_dbContext.Sensors,
                    c => c.Id,
                    s => s.RoomId,
                    (c, s) => new {Classroom = c, Sensor = s})
                .SingleOrDefault((c) => c.Classroom.Name.Equals(roomName) && c.Sensor.Name.Equals(sensorName));
            return sensor?.Sensor;
        }

        public List<Sensor> GetSensors(string roomName)
        {
            var sensors = _dbContext.Classrooms
                .Join(_dbContext.Sensors,
                    c => c.Id,
                    s => s.RoomId,
                    (c, s) => new {Classroom = c, Sensor = s})
                .Where(c => c.Classroom.Name.Equals(roomName))
                .ToList();

            var allSensors = new List<Sensor>();
            foreach (var sensor in sensors)
            {
                var getSensor = sensor.Sensor;
                var currentSensor = new Sensor();
                currentSensor.Id = getSensor.Id;
                currentSensor.Name = getSensor.Name;
                currentSensor.RoomId = getSensor.RoomId;
                currentSensor.Type = getSensor.Type;
                allSensors.Add(currentSensor);
            }

            return allSensors;
        }

        public bool AddData(NewSensorData data)
        {
            try
            {
                var sensor = GetSensor(data.Name, data.Room);

                var newData = new SensorData();
                newData.Id = Guid.NewGuid().ToString();
                newData.Value = data.Value;
                newData.Date = DateTime.Now;
                newData.SensorId = sensor.Id;

                _dbContext.Add(newData);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }

            return true;
        }

        public List<SensorData> GetData(string sensorName, string roomName)
        {
            var sensor = GetSensor(sensorName, roomName);
            var sensorDatas = _dbContext.SensorDatas
                .Where(sd => sd.SensorId.Equals(sensor.Id))
                .ToList();

            return sensorDatas;
        }
    }
}