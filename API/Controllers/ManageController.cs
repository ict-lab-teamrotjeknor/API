using System;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
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
    //Controls every components
    [Route("[controller]")]
    public class ManageController : Controller
    {
        private Manage _manage;
        private JsonEditor _jsonEditor;
        private readonly ILogger _logger;

        public ManageController(ApplicationDbContext dbContext,
            ILogger<ManageController> logger,
            UserManager<User> userManager)
        {
            _manage = new Manage(new DbManage(dbContext, logger), new JsonEditor(logger), logger, userManager);
            _jsonEditor = new JsonEditor(logger);
            _logger = logger;
        }
        
        //Get all rooms
        [HttpGet("getallrooms")]
        public JObject GetAllClassrooms()
        {
            LogUrl();
            return _manage.FindAllClassrooms();
        }
        
        //Add a pi to a room
        [Authorize(Roles = "Admin")]
        [HttpPost("add/pi")]
        public JObject AddPI([FromBody] JObject newPi)
        {
            LogUrl();
            var pi = _jsonEditor.GetPi(newPi);

            var sendBack = _manage.AddPi(pi);
            
            return sendBack;
        }

        //Get all users
        [HttpGet("getusers")]
        public JObject GetUsers()
        {
            LogUrl();
            return _manage.GetUsers();
        }

        //Send a notification to a group
        [HttpPost("sendnotificationgroup")]
        public JObject SendGroupNotification([FromBody] JObject notification)
        {
            LogUrl();
            var notificationMessage = _jsonEditor.GetNotification(notification);   
            var sendBack = _manage.SendGroupNotification(notificationMessage);
            return sendBack;
        }
        
        //Send a notifcation to a member
        [HttpPost("sendnotification")]
        public JObject SendSingleNotification([FromBody] JObject notification)
        {
            LogUrl();
            var notificationMessage = _jsonEditor.GetNotification(notification);   
            var sendBack = _manage.SendNotification(notificationMessage);
            return sendBack;
        }

        //Get all notifications
        [Authorize]
        [HttpGet("notifications")]
        public JObject GetNotifications()
        {
            LogUrl();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _manage.GetNotifications(userId);
        }

        [Authorize]
        [HttpGet("notifications/{id}")]
        public JObject SetReadNotification(string id)
        {
            LogUrl();
            var sendBack = _manage.SetRead(id);
            return sendBack;
        }

        [HttpPost("testpost")]
        public JObject TestPost([FromBody] JObject random)
        {
            LogUrl();
            var test = JObject.Parse(@"{Request:'Post'}");
            return test;
        }
        
        [HttpGet("testget")]
        public JObject TestGet()
        {   
            LogUrl();
            var test = JObject.Parse(@"{Request:'Get'}");
            return test;
        }
        
        [Authorize]
        [HttpGet("testcredentials")]
        public JObject TestCredentials()
        {   
            LogUrl();
            var test = JObject.Parse(@"{Request:'Get'}");
            return test;
        }
        
        [Authorize]
        [HttpPost("testcredentialspost")]
        public JObject TestCredentialsPost([FromBody] JObject random)
        {
            LogUrl();
            var test = JObject.Parse(@"{Request:'Post'}");
            return test;
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