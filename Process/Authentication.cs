using System.Threading.Tasks;
using API.Models;
using API.Process.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace API.Process
{
    public class Authentication
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly SignInManager<DbUser> _signInManager;
        private readonly JsonEditor _json;
        
        public Authentication(
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _json = new JsonEditor();
        }
        
        public async Task<JObject> SignUp(Account newAccount)
        {
            var user = new DbUser { Email = newAccount.Email, UserName = newAccount.Email};
            var result = await _userManager.CreateAsync(user, newAccount.Password);

            var possibleError = new ErrorMessage();
            possibleError.Succeed = result.Succeeded;
            //possibleError.Error = result.Errors.ToString();
            var messsageBack =  _json.SerilizeJObject(possibleError);
            return messsageBack;
        }

        public async Task<JObject> SignIn(Account account)
        {
            var result = await _signInManager.PasswordSignInAsync(account.Email, 
                account.Password, true, lockoutOnFailure: false);
            
            var possibleError = new ErrorMessage();
            possibleError.Succeed = result.Succeeded;
            if (result.IsNotAllowed)
            {
                possibleError.Error = "Email and/or password combination is wrong.";
            }

            return _json.SerilizeJObject(possibleError);
        }
    }
}