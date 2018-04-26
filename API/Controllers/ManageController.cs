﻿using API.Models.Data;
using API.Process;
using API.Process.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ManageController : Controller
    {
        private Manage _manage;
        private JsonEditor _jsonEditor;

        public ManageController(ApplicationDbContext dbContext)
        {
            _manage = new Manage(dbContext);
            _jsonEditor = new JsonEditor();
        }
        
        [HttpGet("getallrooms")]
        public JObject GetAllClassrooms()
        {
            return _manage.FindAllClassrooms();
        }
        
        [HttpPost("add/pi")]
        public JObject AddPI([FromBody] JObject newPi)
        {
            var pi = _jsonEditor.GetPi(newPi);

            var sendBack = _manage.AddPi(pi);
            
            return sendBack;
        }

        [HttpGet("getusers")]
        public JObject GetUsers()
        {
            return _manage.GetUsers();
        }
    }
}