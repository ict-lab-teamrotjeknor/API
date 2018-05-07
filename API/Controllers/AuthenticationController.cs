using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.Data;
using API.Process;
using API.Process.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly Authentication _authentication;
        private readonly JsonEditor _json;
        
        public AuthenticationController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext)
        {
            _authentication = new Authentication(userManager, signInManager, roleManager, dbContext);
            _json = new JsonEditor();
        }
        
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<JObject> SignUp([FromBody] JObject createAccount)
        {
            var newAccount = _json.GetAccount(createAccount);
            var messageBack = _authentication.SignUp(newAccount);
            return await messageBack;
        }
        
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<JObject> SignIn([FromBody] JObject loginAccount)
        {
            var account = _json.GetAccount(loginAccount);
            var messageBack = _authentication.SignIn(account);
            return await messageBack;
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("deleteaccount")]
        public async Task<JObject> DeleteAccount([FromBody] JObject account)
        {
            var deleteAccount = _json.GetAccount(account);
            var messageBack = await _authentication.DeleteAccount(deleteAccount);
            return messageBack;
        }
        
        [AllowAnonymous]
        [HttpGet("createroles")]
        public async Task<JObject> CreateRoles()
        {
            var messageBack = _authentication.CreateRoleAdminAndStudent();
            return await messageBack;
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("addrole")]
        public async Task<JObject> AddRole([FromBody] JObject newRole)
        {
            var messageBack = _authentication.AddNewRole(newRole);
            return await messageBack;
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("changerole")]
        public async Task<JObject> ChangeRole([FromBody] JObject changeRole)
        {
            var messageBack = _authentication.ChangeRole(changeRole);
            return await messageBack;
        }
        
      /*  [HttpGet("currentuser/getrole")]
        public async Task<JObject> UserGetRole()
        {
            
            return messageBack;
        } */
        
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
