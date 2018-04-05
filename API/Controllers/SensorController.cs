using API.Models.Data;
using API.Process;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [Route("[controller]")]
    public class SensorController : Controller
    {
        private SensorHandler _sensorHandler;

        public SensorController(ApplicationDbContext newDbContext)
        {
            _sensorHandler = new SensorHandler(newDbContext);
        }
        
        [HttpPost("addnewsensor")]
        public JObject AddNewSensor([FromBody] JObject newSensor)
        {
            var sendBack = _sensorHandler.AddSensor(newSensor);
            return new JObject();
        }
        
        [HttpGet("getallsensors/{roomName}")]
        public JObject GetAllSensors(string roomName)
        {
            return new JObject();
        }
        
        [HttpGet("getSensorData/{roomName}/{idSensor}")]
        public JObject GetSensorData(string roomName, string idSensor)
        {
            return new JObject();
        }
        
        [HttpPost("addnewsensordata")]
        public JObject AddNewSensorData([FromBody] JObject newData)
        {
            return new JObject();
        }
    }
}