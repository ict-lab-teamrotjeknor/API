using System;
using System.Collections.Generic;
using System.Globalization;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process.Model.Agenda;
using Microsoft.Azure.KeyVault.Models;

namespace UnitTest
{
    public class DbAgendaFake : IDbAgenda
    {
        public Classroom GetClassroom(string classroomName)
        {
            var classroom = new Classroom();
            classroom.Id = "10";
            classroom.Name = "H.4.103";
            return classroom;
        }

        public void SaveClassroom(Classroom newRoom)
        {
            var doNothing = string.Empty;
        }

        public Year GetYear(int year, string roomId)
        {
            var newYear = new Year();
            newYear.Id = "20";
            return newYear;
        }

        public void SaveYear(Year newYear)
        {
            var doNothing = string.Empty;
        }

        public Period GetPeriode(int periode, string yearId)
        {
            var newPeriod = new Period();
            newPeriod.Id = "30";
            return newPeriod;
        }

        public void SavePeriod(Period newPeriod)
        {
            var doNothing = string.Empty;
        }

        public Week GetWeek(int week, string periodId)
        {
             var newWeek = new Week();
            newWeek.Id = "40";
            newWeek.StartWeek = DateTime.ParseExact("2018-10-10 10:40:00,000", "yyyy-MM-dd HH:mm:ss,fff",CultureInfo.InvariantCulture);
            newWeek.EndWeek = DateTime.ParseExact("2018-10-12 10:40:00,000", "yyyy-MM-dd HH:mm:ss,fff",CultureInfo.InvariantCulture);
            return newWeek;
        }

        public void SaveWeek(Week newWeek)
        {
            var doNothing = string.Empty;
        }

        public Day GetDay(string day, string weekId)
        {
            var newDay = new Day();
            newDay.Id = "10";
            return newDay;
        }

        public List<Day> GetDays(string weekId)
        {
            var days = new List<Day>();
            var newDay = new Day();
            newDay.Id = "50";
            newDay.WeekDay = "Maandag";
            days.Add(newDay);
            return days;
        }

        public void SaveDay(Day newDay)
        {
            var doNothing = string.Empty;
        }

        public List<Hour> GetHours(string dayId)
        {
            var hours = new List<Hour>();
            var newHour = new Hour();
            newHour.which = 2;
            newHour.Course = "test";
            newHour.Class = "test2";
            newHour.ShortName = "test3";
            hours.Add(newHour);
            return hours;
        }

        public void SaveHour(Hour newHour)
        {
            var nothing = "something is wrong";
        }

        public string FindHours(NewHour newHour)
        {
            return "10";
        }

        public List<string> GetReservations(string userId)
        {
            var days = new List<string>();
            days.Add("10-09-1994");
            days.Add("11-09-1994");
            return days;
        }

        public MDay GetReservations(string userId, DateTime Day)
        {
            var newDay = new MDay("Dinsdag");
            newDay.Hours[2].Teacher = "test@test.nl";
            newDay.Hours[2].Course = "Student";
            newDay.Hours[2].Reserved = true;
            return newDay;
        }
    }
}