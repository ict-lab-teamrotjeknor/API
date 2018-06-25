using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Controllers;
using API.Models;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json.Linq;

namespace API.Process
{
    //Handles all users and authentications
    public class Authentication
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJsonEditor _json;
        private readonly IDbManage _dbManage;
        private readonly ILogger _logger;
        private bool deployed;
        
        public Authentication(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext,
            ILogger logger)
        {
            deployed = true;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _json = new JsonEditor(logger);
            _dbManage = new DbManage(dbContext, logger);
            _logger = logger;
        }

        public Authentication(IDbManage newDbManage, IJsonEditor newJson)
        {
            deployed = false;
            _dbManage = newDbManage;
            _json = newJson;
        }
        
        //Make users
        public async Task<JObject> SignUp(Account newAccount)
        {
            var user = new User { Email = newAccount.Email, UserName = newAccount.Email};
            var result = await _userManager.CreateAsync(user, newAccount.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");
            }

            var possibleError = new ErrorMessage();
            possibleError.Succeed = result.Succeeded;
            if (!possibleError.Succeed)
            {
                possibleError.Error = result.Errors.First().Description.ToString();
                _logger.LogError(result.Errors.First().Description.ToString());
            }
            var messsageBack =  _json.SerilizeJObject(possibleError);
            return messsageBack;
        }

        //Login the users
        public async Task<JObject> SignIn(Account account)
        {
            var access = false;
            var user = await _userManager.FindByNameAsync(account.Email);
            if (user != null)
            {
                access = !user.Delete;
            }

            if (access)
            {
                var result = await _signInManager.PasswordSignInAsync(account.Email,
                    account.Password, true, lockoutOnFailure: false);
                access = result.Succeeded;
            }
            else
            {
                _logger.LogInformation("Account deleted");
            }

            var possibleError = new ErrorMessage();
            possibleError.Succeed = access;
            if (!access)
            {
                possibleError.Error = "Email and/or password combination is wrong.";
                _logger.LogInformation(possibleError.Error);
            }

            return _json.SerilizeJObject(possibleError);
        }

        //Make standard roles first time
        public async Task<JObject> CreateRoleAdminAndStudent()
        {   
            var exists = await _roleManager.RoleExistsAsync("Admin");

            if (exists)
            {
                _logger.LogError("Admin role already exists");
                return _json.GetError("Admin role already exists");
            }
            
            var adminRole = new IdentityRole { Name = "Admin" };
            await _roleManager.CreateAsync( adminRole ); 
            
            var user = new User { UserName = "admin@admin.nl", Email = "admin@admin.nl" };
                var result = await _userManager.CreateAsync(user, "Test1234:)");

            await _userManager.AddToRoleAsync(user, "Admin");
            
            exists = await _roleManager.RoleExistsAsync("Student");

            if (exists)
            {
                _logger.LogError("Student role already exists");
                return _json.GetError("Student role already exists");
            }
            
            var studentRole = new IdentityRole { Name = "Student" };
            await _roleManager.CreateAsync( studentRole );

            return _json.GetSucced();
        }

        //Create a now role
        public async Task<JObject> AddNewRole(JObject newRole)
        {
            var role = _json.GetRole(newRole);
            
            var exists = await _roleManager.RoleExistsAsync(role.RoleName);

            if (exists)
            {
                _logger.LogInformation("role already exists");
                return _json.GetError(role.RoleName + " role already exists");
            }
            
            var identityRole = new IdentityRole { Name = role.RoleName };
            await _roleManager.CreateAsync( identityRole );
            
            return _json.GetSucced();
        }

        //Change somebodies role
        public async Task<JObject> ChangeRole(JObject changeRole)
        {
            var role = _json.GetRole(changeRole);
            
            var user = await _userManager.FindByNameAsync(role.UserEmail);

            var currentRole = await _userManager.GetRolesAsync(user);
            
            var resultRemove = await _userManager.RemoveFromRoleAsync(user, currentRole[0].ToString());

            if (resultRemove.Succeeded)
            {
                var resultAdd = await _userManager.AddToRoleAsync(user, role.RoleName);

                if (resultAdd.Succeeded)
                {
                    return _json.GetSucced();
                }
                else
                {
                    _logger.LogInformation(resultRemove.Errors.First().Description);
                    await _userManager.AddToRoleAsync(user, currentRole[0]);
                    return _json.GetError(resultAdd.Errors.First().Description);
                }
            }
            else
            {
                _logger.LogInformation(resultRemove.Errors.First().Description);
                return _json.GetError(resultRemove.Errors.First().Description);
            }
        }

        //Disable a account
        public async Task<JObject> DeleteAccount(Account deleteAccount)
        {
            var result = _dbManage.DeleteUser(deleteAccount);
            if (result)
            {
                return _json.GetSucced();
            }
            else
            {
                _logger.LogInformation("Account doesn't exists");
                return _json.GetError("Account " + deleteAccount.Email + " doesn't exists");
            }
            
        }

        //Check somebodies role
        public async Task<JObject> CheckRole(string UserEmail)
        {
            var user = await _userManager.FindByNameAsync(UserEmail);
            if (user == null)
            {
                _logger.LogInformation("Account doesn't exists");
                return _json.GetError("Account " + user.Email + " doesn't exists");
            }
            var currentRole = await _userManager.GetRolesAsync(user);
            var userRole = new Role();
            userRole.RoleName = currentRole[0];
            userRole.UserEmail = UserEmail;
            return _json.SerilizeJObject(userRole);
        } 
    }
}