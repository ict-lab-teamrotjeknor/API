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
            _dbSensor.SaveSensor(newsensor, room);
            return new JObject();
        }
    }
}