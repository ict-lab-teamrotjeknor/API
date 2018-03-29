using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Process;
using API.Process.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly Authentication _authentication;
        private readonly JsonEditor _json;
        
        public AuthenticationController(
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager)
        {
            _authentication = new Authentication(userManager, signInManager);
            _json = new JsonEditor();
        }
        
        [HttpPost("signup")]
        public async Task<JObject> SignUp([FromBody] JObject createAccount)
        {
            var newAccount = _json.GetAccount(createAccount);
            var messageBack = _authentication.SignUp(newAccount);
            return await messageBack;
        }
        
        [HttpPost("signin")]
        public async Task<JObject> SignIn([FromBody] JObject loginAccount)
        {
            var account = _json.GetAccount(loginAccount);
            var messageBack = _authentication.SignIn(account);
            return await messageBack;
        }
        
        /*
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        
        */
    }
}
