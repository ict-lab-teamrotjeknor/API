using API.Process;
using API.Process.Model;
using Newtonsoft.Json.Linq;
using Xunit;

namespace UnitTest
{
        public class ManageTest
    {
        private Manage _manage;

        public ManageTest()
        {
            _manage = new Manage(new DbManageFake(), new JsonEditorFake(), null, null, false);
        }

        [Fact]
        public void FindClassrooms()
        {
            var result = _manage.FindAllClassrooms();
            var expected = JObject.Parse(@"{
                'Classroom': {
                         '1': {
                             'Id': '0',
                             'Name': 'Name0'
                         },
                         '2': {
                             'Id': '1',
                             'Name': 'Name1'
                         }
                     }
                 }");
            
            Assert.Equal(expected.ToString(), result.ToString());
        }

        [Fact]
        public void AddPiFalse()
        {
            var currentPi = new NewPI();
            currentPi.ClassroomName = "H.4.103";
            currentPi.MacAdress = "103:103";
            
            var result = _manage.AddPi(currentPi);

            var expected = JObject.Parse(@"{'Succeed':false}");
            
            Assert.Equal(expected.ToString(), result.ToString());
        }
        
        
        [Fact]
        public void AddPiTrue()
        {
            var currentPi = new NewPI();
            currentPi.ClassroomName = "H.4.104";
            currentPi.MacAdress = "103:103";
            
            var result = _manage.AddPi(currentPi);

            var expected = JObject.Parse(@"{'Succeed':true}");
            
            Assert.Equal(expected.ToString(), result.ToString());
        }

        [Fact]
        public void GetUsers()
        {
            var result = _manage.GetUsers();

            var expected = JObject.Parse(@"{
            'Users': [
            {
                'Id': '13',
                'Email': 'test@test.nl'
            }
            ]
        }");
            
        Assert.Equal(expected.ToString(), result.ToString());
        }
    }
}