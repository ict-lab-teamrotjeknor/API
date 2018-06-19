using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.Data;
using API.Process;
using API.Process.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    //It's a controller for all authentications of users
    [Authorize]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly Authentication _authentication;
        private readonly JsonEditor _json;
        private readonly ILogger _logger;
        
        public AuthenticationController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext,
            ILogger<AuthenticationController> logger)
        {
            _authentication = new Authentication(userManager, signInManager, roleManager, dbContext, logger);
            _json = new JsonEditor(logger);
            _logger = logger;
        }
        
        //Make a new account
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<JObject> SignUp([FromBody] JObject createAccount)
        {
            LogUrl();
            var newAccount = _json.GetAccount(createAccount);
            var messageBack = _authentication.SignUp(newAccount);
            return await messageBack;
        }
        
        //Inloggen
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<JObject> SignIn([FromBody] JObject loginAccount)
        {
            LogUrl();
            var account = _json.GetAccount(loginAccount);
            var messageBack = _authentication.SignIn(account);
            return await messageBack;
        }
        
        //Delete a user
        [Authorize(Roles = "Admin")]
        [HttpPost("deleteaccount")]
        public async Task<JObject> DeleteAccount([FromBody] JObject account)
        {
            LogUrl();
            var deleteAccount = _json.GetAccount(account);
            var messageBack = await _authentication.DeleteAccount(deleteAccount);
            return messageBack;
        }
        
        //Make a the standard roles
        [AllowAnonymous]
        [HttpGet("createroles")]
        public async Task<JObject> CreateRoles()
        {
            LogUrl();
            var messageBack = _authentication.CreateRoleAdminAndStudent();
            return await messageBack;
        }
        
        //Make a new role
        [Authorize(Roles = "Admin")]
        [HttpPost("addrole")]
        public async Task<JObject> AddRole([FromBody] JObject newRole)
        {
            LogUrl();
            var messageBack = _authentication.AddNewRole(newRole);
            return await messageBack;
        }
        
        //Change a userrole
        [Authorize(Roles = "Admin")]
        [HttpPost("changerole")]
        public async Task<JObject> ChangeRole([FromBody] JObject changeRole)
        { 
            LogUrl();
            var messageBack = _authentication.ChangeRole(changeRole);
            return await messageBack;
        }
        
        //CheckSomebodies role
        [HttpGet("checkrole/{username}")]
        public async Task<JObject> CheckRole(string username)
        {

            LogUrl();;
            var messageBack = _authentication.CheckRole(username);
            return await messageBack;
        }

        //Safe actions from someone
        private void LogUrl()
        {
            var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
            var url = location.AbsoluteUri;
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            _logger.LogInformation(remoteIpAddress + ": " + url);
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
