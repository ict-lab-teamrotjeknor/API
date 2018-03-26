using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ScheduleController : Controller
    {
        [HttpPost("uploadnewweek")]
        public JObject UploadWeek([FromBody] JObject weekSchedule)
        {
            Console.WriteLine("TEST!!");
            Console.WriteLine(weekSchedule);
            return weekSchedule;
        }
    }
}