using System;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process;
using API.Process.Model.Agenda;
using Newtonsoft.Json.Linq;
using Xunit;

namespace UnitTest
{
    public class AgendaTest
    {
        private Agenda _agenda;
        private IJsonEditor _json;

        public AgendaTest()
        {
            var jsonEditor = new JsonEditorFake();
            _json = jsonEditor;
            var  dbAgenda = new DbAgendaFake();
            _agenda = new Agenda(dbAgenda, jsonEditor);
        }

        [Fact]
        public void UploadNewMonth()
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
            var schedule = _json.SerilizeJObject(newSchedule);
            var result = _agenda.Upload(schedule);
            
            var expected = JObject.Parse(@"{'Succeed':true}");
            
            Assert.Equal(expected.ToString(), result.ToString());
        }

        [Fact]
        public void UploadnewHour()
        {
            var currentHour = JObject.Parse(@"{
            'Username':'test@test.nl',
            'Type':'Student',
            'Classroom':'H.1.308',
            'Week':22,
            'StartHour':4,
            'TotalHours':2,
            'Day':'Dinsdag',
            'Year':2018
            }
            ");

            var result = _agenda.NewHour(currentHour);
            
            var expected = JObject.Parse(@"{'Succeed':true}");
            
            Assert.Equal(expected.ToString(), result.ToString());
            
        }
        
        [Fact]
        public void GetWeek()
        {
            var roomName = "H.4.103";
            var year = 2018;
            var weekNumber = 22;

            var result = _agenda.GetWeek(roomName, year, weekNumber);

            var expected = JObject.Parse(@"
            {
            'ClassroomName': 'H.4.103',
            'ClassroomType': null,
            'StartDate': '10/10/18 10:40:00 AM',
            'EndDate': '10/12/18 10:40:00 AM',
            'SchoolType': null,
            'Days': [
            {
                'Name': 'Maandag',
                'Hours': [
                {
                    'HourId': 2,
                    'Class': 'test2',
                    'Teacher': 'test3',
                    'Course': 'test',
                    'SpecialReason': null,
                    'Reserved': true
                }]}]}
                ");
               
            Assert.Equal(expected.ToString(), result.ToString());
        }

        [Fact]
        public void GetPersionalReservations()
        {
            var userId = "10";
            var result = _agenda.GetPersonalReservations(userId);

            var expected = JObject.Parse(@"
                {    
                'random': [
                    '10-09-1994',
                    '11-09-1994'
                ]
                }
                ");
            
            Assert.Equal(expected.ToString(), result.ToString());
        }

        [Fact]
        public void GetPersionalReservationsDay()
        {
            var userid = "10";
            var day = "2018-08-10";

            var result = _agenda.GetPersonalReservations(userid, day);

            var expected = JObject.Parse(@"{
            'Name': 'Dinsdag',
            'Hours': [
            {
                'HourId': 3,
                'Class': null,
                'Teacher': 'test@test.nl',
                'Course': 'Student',
                'SpecialReason': null,
                'Reserved': true
            }
            ]}");
            
            Assert.Equal(expected.ToString(), result.ToString());
        }
    }
}
