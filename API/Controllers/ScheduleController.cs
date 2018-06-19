using System;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process;
using API.Process.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
   // [Authorize]
    [Route("[controller]")]
    public class ScheduleController : Controller
    {
        private Agenda _agenda;
        private UserManager<User> _userManager;
        private readonly ILogger _logger;

        public ScheduleController(ApplicationDbContext dbContext, UserManager<User> userManager,
            ILogger<ScheduleController> logger)
        {
            var dbAgenda = new DbAgenda(dbContext, logger);
            var jsonEditor = new JsonEditor(logger);
            _agenda = new Agenda(dbAgenda, jsonEditor, logger);
            _userManager = userManager;
            _logger = logger;

        }
        
        //Upload a new week from hint
        [HttpPost("uploadnewweek")]
        public JObject UploadWeek([FromBody] JObject weekSchedule)
        {
            LogUrl();
            var sendBack = _agenda.Upload(weekSchedule);
            
            return sendBack;
        }

        //Get a week from a room
        [HttpGet("getweek/{roomId}/{year}/{weekNumber}")]
        public JObject GetWeek(string roomId, int year, int weeknumber)
        {
            LogUrl();
            var sendBack = _agenda.GetWeek(roomId, year, weeknumber);
            return sendBack;
        }

        //upload a hour to a room
        [HttpPost("uploadhour")]
        public JObject UploadHour([FromBody] JObject hourSchedule)
        {
            LogUrl();
            var sendBack = _agenda.NewHour(hourSchedule);
            return sendBack;
        }

        //Get all days of personal users
        [HttpGet("getuserreservations")]
        public JObject GetUserReservations()
        {
            LogUrl();
            var userId = GetCurrentUser();
            var sendBack = _agenda.GetPersonalReservations(userId);
            return sendBack;
        }

        //Get detail days of personal users
        [HttpGet("getuserreservations/{day}")]
        public JObject GetUserReservations(string day)
        {
            LogUrl();
            var userId = GetCurrentUser();
            var sendBack = _agenda.GetPersonalReservations(userId, day);
            return sendBack;
        }
        
        private string GetCurrentUser()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
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