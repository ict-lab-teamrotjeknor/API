using System.Collections.Generic;
using API.Models.Data;
using API.Process;
using API.Process.Model;
using API.Process.Model.Agenda;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnitTest
{
    public class JsonEditorFake : IJsonEditor
    {
        public Account GetAccount(JObject account)
        {
            throw new System.NotImplementedException();
        }

        public JObject SerilizeJObject(object serilizeObject)
        {
            var stringJson = JsonConvert.SerializeObject(serilizeObject);
            var sendback = JObject.Parse(stringJson);
            return sendback;
        }

        public Schedule GetSchedule(JObject schedule)
        {
            var newSchedule = new Schedule();
            newSchedule.ClassroomName = "H.3.103";
            newSchedule.ClassroomType = "School";
            newSchedule.StartDate = "2018-05-28 00:00:00,000";
            newSchedule.EndDate = "2018-05-28 00:00:00,000";
            newSchedule.AddDay("Maandag");
            newSchedule.Days[0].Hours[0].Class = "BBVIP";
            newSchedule.Days[0].Hours[0].Teacher = "BBVIP";
            newSchedule.Days[0].Hours[0].Course = "BBVIP";
            newSchedule.Days[0].Hours[0].Reserved = true;
            return newSchedule;
        }

        public Role GetRole(JObject role)
        {
            throw new System.NotImplementedException();
        }

        public NewPI GetPi(JObject newPi)
        {
            throw new System.NotImplementedException();
        }

        public NewSensorData GetSensorData(JObject newData)
        {
            throw new System.NotImplementedException();
        }

        public JObject MakeClassrooms(List<Classroom> classrooms)
        {
            var classroomsList = JObject.Parse(@"{
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
            return classroomsList;
        }

        public Sensor GetSensor(JObject sensor)
        {
            return new Sensor();
        }

        public NewHour GetNewHour(JObject newHour)
        {
            var currentHour = new NewHour();
            currentHour.Username = "test@test.nl";
            currentHour.Type = "Student";
            currentHour.Classroom = "H.1.103";
            currentHour.Week = 22;
            currentHour.StartHour = 4;
            currentHour.TotalHours = 2;
            currentHour.Day = "Friday";
            currentHour.Year = 2018;
            return currentHour;
        }

        public string GetRoom(JObject sensor)
        {
            return "room";
        }

        public JObject GetError(string errorMessage)
        {
             return JObject.Parse(@"{'Succeed':false}");
        }

        public JObject GetSucced()
        {
            return JObject.Parse(@"{'Succeed':true}");
        }
    }
}