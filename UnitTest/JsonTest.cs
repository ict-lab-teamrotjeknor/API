using System.Collections.Generic;
using API.Process.Model;
using API.Models.Data;
using API.Process.Model.Agenda;
using Newtonsoft.Json.Linq;
using Xunit;
 
 namespace UnitTest
 {
     public class JsonTest
     {
         private JsonEditor _json;
 
         public JsonTest()
         {
             _json = new JsonEditor();
         }
         
         [Fact]
         public void GetAccount()
         {
             var currentAccount = JObject.Parse(@"{ 
                 'Email':'test@test.nl',
                 'Password':'Test1234'}"
             );
 
             var result = _json.GetAccount(currentAccount);
             var expected = new Account();
             expected.Email = "test@test.nl";
             expected.Password = "Test1234";
             
             Assert.Equal(expected.Email, result.Email);
             Assert.Equal(expected.Password, result.Password);
         }

         [Fact]
         public void SerilizeJObject()
         {
             var currenttest = new Account();
             currenttest.Email = "test@test.nl";
             currenttest.Password = "Test1234";

             var result = _json.SerilizeJObject(currenttest);
             
             var expectedAccount = JObject.Parse(@"{ 
                 'Email':'test@test.nl',
                 'Password':'Test1234',
                 'Confirm':null}"
             );
             
             Assert.Equal(expectedAccount.ToString(), result.ToString());
         }

         [Fact]
         public void GetSchedule()
         {
             var currentSchedule = JObject.Parse(@"{
             'ClassroomName': 'H.3.308',
             'ClassroomType': 'Theorielokaal',
             'StartDate': '2018-05-28 00:00:00,000',
             'EndDate': '2018-06-03 00:00:00,000',
             'SchoolType': null,
             'Days': [
             {
                 'Name': 'Maandag',
                 'Hours': [
                 {
                     'HourId': 1,
                     'Class': 'COV3E',
                     'Teacher': 'AUGUM',
                     'Course': '<b>CCODKR10R3</b>',
                     'SpecialReason': null,
                     'Reserved': true
                 },
                 {
                     'HourId': 2,
                     'Class': 'COV3E',
                     'Teacher': 'AUGUM',
                     'Course': '<b>CCODKR10R3</b>',
                     'SpecialReason': null,
                     'Reserved': true
                 }
                 ]
             },
             {
                'Name': 'Dinsdag',
                 'Hours': []
             },
             {
                 'Name': 'Woensdag',
                 'Hours': []
             },
             {
                 'Name': 'Donderdag',
                 'Hours': []
             },
             {
                 'Name': 'Vrijdag',
                 'Hours': []
             }
             ]
         }");

             var result = _json.GetSchedule(currentSchedule);

             var classroomName = "H.3.308";
             var classroomType = "Theorielokaal";
             var StartDate = "2018-05-28 00:00:00,000";
             var EndDate = "2018-06-03 00:00:00,000";

             var HourId = 1;
             var Class = "COV3E";
             var Teacher = "AUGUM";
             var Course = "<b>CCODKR10R3</b>";
             var Reserved = true;
             
             Assert.Equal(classroomName, result.ClassroomName);
             Assert.Equal(classroomType, result.ClassroomType);
             Assert.Equal(StartDate, result.StartDate);
             Assert.Equal(EndDate, result.EndDate);

             var currentHour = result.Days[0].Hours[0];
             
             Assert.Equal(HourId, currentHour.HourId);
             Assert.Equal(Class, currentHour.Class);
             Assert.Equal(Teacher, currentHour.Teacher);
             Assert.Equal(Course, currentHour.Course);
             Assert.Equal(Reserved, currentHour.Reserved);
         }

         [Fact]
         public void GetRole()
         {
             var currentRole = JObject.Parse(@"{ 
                 'RoleName':'Student',
                 'UserEmail':'test@test.nl'}"
             );

             var result = _json.GetRole(currentRole);
             
             var expected = new Role();
             expected.RoleName = "Student";
             expected.UserEmail = "test@test.nl";
             
             Assert.Equal(expected.RoleName, result.RoleName);
             Assert.Equal(expected.UserEmail, expected.UserEmail);
         }

         [Fact]
         public void GetPi()
         {
             var currentPi = JObject.Parse(
                 @"{ 
                 'Id':'1234',
                 'MacAdress':'123:123:123:123',
                 'ClassroomName':'H.4.103'}"
             );

             var result = _json.GetPi(currentPi);
             
             var expected = new NewPI();
             expected.Id = "1234";
             expected.MacAdress = "123:123:123:123";
             expected.ClassroomName = "H.4.103";
             
             Assert.Equal(expected.Id, result.Id);
             Assert.Equal(expected.MacAdress, result.MacAdress);
             Assert.Equal(expected.ClassroomName, result.ClassroomName);
         }

         [Fact]
         public void GetSensorData()
         {
             var currentSensorData = JObject.Parse(
                 @"{
                    'Name':'Temperature',
                    'Value':'14',
                    'Room':'H.4.103'
                    }"
             );
             
             var result = _json.GetSensorData(currentSensorData);
             
             var expected = new NewSensorData();
             expected.Name = "Temperature";
             expected.Value = "14";
             expected.Room = "H.4.103";
             
             Assert.Equal(expected.Name, result.Name);
             Assert.Equal(expected.Value, result.Value);
             Assert.Equal(expected.Room, result.Room);
         }

         [Fact]
         public void MakeClassrooms()
         {
             var classrooms = new List<Classroom>();

             for (int i = 0; i < 2; i++)
             {
                 var newClassroom = new Classroom();
                 newClassroom.BuildingId = "1234";
                 newClassroom.Id = i.ToString();
                 newClassroom.Name = "Name" + i;
                 newClassroom.PiID = "PI" + i;
                 classrooms.Add(newClassroom);
             }

             var result = _json.MakeClassrooms(classrooms);

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
         public void GetSensor()
         {
             var currentSensor = JObject.Parse(
                 @"{
                    'Name':'Nice',
                    'Type':'Temperature',
                    'RoomId':'1234'
                    }"
             );
             
             var result = _json.GetSensor(currentSensor);
             
             var expected = new Sensor();
             expected.Name = "Nice";
             expected.Type = "Temperature";
             expected.RoomId = "1234";
             
             Assert.Equal(expected.Name, result.Name);
             Assert.Equal(expected.Type, result.Type);
             Assert.Equal(expected.RoomId, result.RoomId);
         }

         [Fact]
         public void GetHour()
         {
             var currentHour = JObject.Parse(
                 @"{
                    'Username':'admin@admin.nl',
                    'Type':'Theorie',
                    'Classroom':'Room',
                    'Week':'22',
                    'StartHour':'3',
                    'TotalHours':'2',
                    'Day':'Friday',
                    'Year':'2018'
                    }"
             );
                 
             var result = _json.GetNewHour(currentHour);
             var expected = new NewHour();
             expected.Username = "admin@admin.nl";
             expected.Type = "Theorie";
             expected.Classroom = "Room";
             expected.Week = 22;
             expected.StartHour = 3;
             expected.TotalHours = 2;
             expected.Day = "Friday";
             expected.Year = 2018;

             Assert.Equal(expected.Username, result.Username);
             Assert.Equal(expected.Type, result.Type);
             Assert.Equal(expected.Classroom, result.Classroom);
             Assert.Equal(expected.Week, result.Week);
             Assert.Equal(expected.StartHour, result.StartHour);
             Assert.Equal(expected.TotalHours, result.TotalHours);
             Assert.Equal(expected.Day, result.Day);
             Assert.Equal(expected.Year, result.Year);
         }

         [Fact]
         public void GetRoom()
         {
             var currentSensor = JObject.Parse(@"{'Room':'H.4.103'}");
             var result = _json.GetRoom(currentSensor);
             var expected = "H.4.103";
             
             Assert.Equal(expected, result);
         }

         [Fact]
         public void GetError()
         {
             var currentError = "This isnot good";

             var result = _json.GetError(currentError);
             
             var expected = JObject.Parse(@"{'Succeed':false,
                                              'Error':'This isnot good'}");
             
             Assert.Equal(expected.ToString(), result.ToString());
         }

         [Fact]
         public void GetSucced()
         {
             var result = _json.GetSucced();
             
             var expected = JObject.Parse(@"{'Succeed':true,
                                              'Error':''}");
             
             Assert.Equal(expected.ToString(), result.ToString());
         }
     }
 }