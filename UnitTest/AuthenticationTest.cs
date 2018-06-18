using System;
using API.Process;
using API.Process.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json.Linq;
using Xunit;

namespace UnitTest
{
    public class AuthenticationTest
    {
        private Authentication _authentication;
        
        public AuthenticationTest()
        {
            _authentication = new Authentication(new DbManageFake(), new JsonEditorFake());
        }

        [Fact]
        public void DeleteUser()
        {
            var newAccount = new Account();
            newAccount.Email = "test@test.nl";
            var result = _authentication.DeleteAccount(newAccount);
            
            JObject newResult = result.Result;
            
            var expected = JObject.Parse(@"{'Succeed':true}");
            
            Assert.Equal(expected.ToString(), newResult.ToString());
        }
        
    }
}