using System;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process;
using API.Process.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
   // [Authorize]
    [Route("[controller]")]
    public class ScheduleController : Controller
    {
        private Agenda _agenda;

        public ScheduleController(ApplicationDbContext dbContext)
        {
            var dbAgenda = new DbAgenda(dbContext);
            var jsonEditor = new JsonEditor();
            _agenda = new Agenda(dbAgenda, jsonEditor);
        }
        
        [HttpPost("uploadnewweek")]
        public JObject UploadWeek([FromBody] JObject weekSchedule)
        {
            var sendBack = _agenda.Upload(weekSchedule);
            
            return sendBack;
        }
        
        [HttpGet("getweek/{roomId}/{year}/{kwartaal}/{weekNumber}")]
        public JObject GetWeek(string roomId, int year, int kwartaal, int weeknumber)
        {
            var sendBack = _agenda.GetWeek(roomId, year, kwartaal, weeknumber);
            return sendBack;
        }

        [HttpPost("uploadhour")]
        public JObject UploadHour([FromBody] JObject hourSchedule)
        {
            var sendBack = _agenda.NewHour(hourSchedule);
            return sendBack;
        }
    }
}