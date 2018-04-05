using API.Models.Data;
using API.Process;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ManageController : Controller
    {
        private Manage _manage;

        public ManageController(ApplicationDbContext dbContext)
        {
            _manage = new Manage(dbContext);
        }
        
        [HttpGet("getallrooms")]
        public JObject GetAllClassrooms()
        {
            return _manage.FindAllClassrooms();
        }
    }
}