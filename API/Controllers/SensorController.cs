using API.Models.Data;
using API.Process;
using API.Process.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [Route("[controller]")]
    public class SensorController : Controller
    {
        private SensorHandler _sensorHandler;
        private JsonEditor _jsonEditor;

        public SensorController(ApplicationDbContext newDbContext)
        {
            _sensorHandler = new SensorHandler(newDbContext);
            _jsonEditor = new JsonEditor();
        }
        
        [HttpPost("addnewsensor")]
        public JObject AddNewSensor([FromBody] JObject newSensor)
        {
            var sendBack = _sensorHandler.AddSensor(newSensor);
            return sendBack;
        }
        
        [HttpGet("getallsensors/{roomName}")]
        public JObject GetAllSensors(string roomName)
        {
            var sendBack = _sensorHandler.GetSensors(roomName);
            return sendBack;
        }
        
        [HttpGet("getsensordata/{roomName}/{sensorName}")]
        public JObject GetSensorData(string roomName, string sensorName)
        {
            var sendBack = _sensorHandler.GetData(roomName, sensorName);
            return sendBack;
        }
        
        [HttpPost("adddata")]
        public JObject AddNewSensorData([FromBody] JObject newData)
        {
            var sensorData = _jsonEditor.GetSensorData(newData);
            var sendBack = _sensorHandler.AddData(sensorData);
            return sendBack;
        }
    }
}