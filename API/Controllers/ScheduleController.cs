﻿using System;
using API.Models.Data;
using API.Process;
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
            _agenda = new Agenda(dbContext);
        }
        
        [HttpPost("uploadnewweek")]
        public JObject UploadWeek([FromBody] JObject weekSchedule)
        {
            var sendBack = _agenda.Upload(weekSchedule);
            
            return sendBack;
        }
        
        [HttpGet("getweek/{roomId}")]
        public JObject GetWeek(string roomId)
        { 
            return new JObject();
        }
    }
}