using API.Models.Data;
using API.Process;
using API.Process.Model;
using Microsoft.AspNetCore.Authorization;
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
        
        [Authorize(Roles = "Admin")]
        [HttpPost("add/pi")]
        public JObject AddPI([FromBody] JObject newPi)
        {
            var pi = _jsonEditor.GetPi(newPi);

            var sendBack = _manage.AddPi(pi);
            
            return sendBack;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("getusers")]
        public JObject GetUsers()
        {
            return _manage.GetUsers();
        }

        [HttpPost("testpost")]
        public JObject TestPost([FromBody] JObject random)
        {
            var test = JObject.Parse(@"{Request:'Post'}");
            return test;
        }
        
        [HttpPost("testget")]
        public JObject TestGet()
        {
            var test = JObject.Parse(@"{Request:'Get'}");
            return test;
        }
    }
}