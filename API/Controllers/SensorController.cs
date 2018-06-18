using System;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process;
using API.Process.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [Route("[controller]")]
    public class SensorController : Controller
    {
        private SensorHandler _sensorHandler;
        private JsonEditor _jsonEditor;
        private readonly ILogger _logger;

        public SensorController(ApplicationDbContext newDbContext, ILogger<SensorController> logger)
        {
            _sensorHandler = new SensorHandler(new DbSensor(newDbContext, logger), logger);
            _jsonEditor = new JsonEditor(logger);
            _logger = logger;
        }
        
        [HttpPost("addnewsensor")]
        public JObject AddNewSensor([FromBody] JObject newSensor)
        {
            LogUrl();
            var sendBack = _sensorHandler.AddSensor(newSensor);
            return sendBack;
        }
        
        [HttpGet("getallsensors/{roomName}")]
        public JObject GetAllSensors(string roomName)
        {
            LogUrl();
            var sendBack = _sensorHandler.GetSensors(roomName);
            return sendBack;
        }
        
        [HttpGet("getsensordata/{roomName}/{sensorName}")]
        public JObject GetSensorData(string roomName, string sensorName)
        {
            LogUrl();
            var sendBack = _sensorHandler.GetData(roomName, sensorName);
            return sendBack;
        }
        
        [HttpPost("adddata")]
        public JObject AddNewSensorData([FromBody] JObject newData)
        {
            LogUrl();
            var sensorData = _jsonEditor.GetSensorData(newData);
            var sendBack = _sensorHandler.AddData(sensorData);
            return sendBack;
        }
        
        private void LogUrl()
        {
            var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
            var url = location.AbsoluteUri;
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            _logger.LogInformation(remoteIpAddress + ": " + url);
        }
    }
}