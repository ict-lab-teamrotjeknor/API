using System;
using System.Globalization;
using System.Net.Mime;
using System.Threading.Tasks.Dataflow;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process.Model;
using API.Process.Model.Agenda;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Day = API.Models.Data.Day;
using Hour = API.Models.Data.Hour;

namespace API.Process
{
    //It's class for handeling data for the agenda
    public class Agenda 
    {
        private IDbAgenda _dbAgenda;
        private IJsonEditor _jsonEditor;
        private readonly ILogger _logger;
        private bool Deployment;

        public Agenda(IDbAgenda dbAgenda, IJsonEditor jsonEditor, ILogger logger)
        {
            Deployment = true;
            _dbAgenda = dbAgenda;
            _jsonEditor = jsonEditor;
            _logger = logger;
        }
        
        public Agenda(IDbAgenda dbAgenda, IJsonEditor jsonEditor)
        {
            Deployment = false;
            _dbAgenda = dbAgenda;
            _jsonEditor = jsonEditor;
        }
        
        //Upload a new week
        public JObject Upload(JObject newWeek)
        {
            var schedule = _jsonEditor.GetSchedule(newWeek);
            NewClassroom(schedule);

            return _jsonEditor.GetSucced();
        }

        //Upload a new hour && checked if it already exists
        public JObject NewHour(JObject newHour)
        {
            var sendBack = new JObject();
            var hours = _jsonEditor.GetNewHour(newHour);
            hours.Quator = 4;
            var found = _dbAgenda.FindHours(hours);

            if (!found.Equals(string.Empty))
            {
                for (var i = 0; i < hours.TotalHours; i++) {
                    var saveHour = new Hour();
                    saveHour.Class = String.Empty;
                    saveHour.Course = hours.Type;
                    saveHour.ScheduleDayId = found;
                    saveHour.ShortName = hours.Username;
                    saveHour.which = hours.StartHour + i;
                    _dbAgenda.SaveHour(saveHour);
                }

                sendBack = _jsonEditor.GetSucced();
            }
            else
            {
                if (Deployment) _logger.LogInformation("Hour already exists");
                sendBack = _jsonEditor.GetError("Hour already exists");
            }
            return sendBack;
        }

        //Get a week of a room
        public JObject GetWeek(string roomName, int year, int weekNumber)
        {
            var classroom = _dbAgenda.GetClassroom(roomName);

            if (classroom != null)
            {
                var yearModel = _dbAgenda.GetYear(year, classroom.Id);
                var kwartaalModel = _dbAgenda.GetPeriode(4, yearModel.Id);
                var week = _dbAgenda.GetWeek(weekNumber, kwartaalModel.Id);

                var schedule = new Schedule();
                schedule.ClassroomName = classroom.Name;
                schedule.StartDate = week.StartWeek.ToString(CultureInfo.CurrentCulture);
                schedule.EndDate = week.EndWeek.ToString(CultureInfo.CurrentCulture);
                schedule = GetDays(schedule, week);
                schedule = RemoveZeroHours(schedule);
                return _jsonEditor.SerilizeJObject(schedule);
            }
            else
            {
                if (Deployment) _logger.LogInformation("Classroom already exists");
                return _jsonEditor.GetError("Classroom already exists");
            }
        }

        //Get All days of personal resovations
        public JObject GetPersonalReservations(string userId)
        {
            var days = _dbAgenda.GetReservations(userId);
            var setJsonList = new JsonList();
            setJsonList.random = days;
            var sendBack = _jsonEditor.SerilizeJObject(setJsonList);
            return sendBack;
        }
        
        //Get all details of one day of persional resesvationts
        public JObject GetPersonalReservations(string userId, string day)
        {
            day = day + " 00:00:00,000";
            var realDay = DateTime.ParseExact(day, "yyyy-MM-dd HH:mm:ss,fff",
                CultureInfo.InvariantCulture);
            var detailDay = _dbAgenda.GetReservations(userId, realDay);
            detailDay = RemoveZeroHoursDay(detailDay);
            
            var sendBack = _jsonEditor.SerilizeJObject(detailDay);
            return sendBack;
        }

        //Get All days
        private Schedule GetDays(Schedule schedule, Week week)
        {
            var days = _dbAgenda.GetDays(week.Id);

            var currentDay = 0;
            
            foreach (var day in days)
            {
                schedule.AddDay(day.WeekDay);
                schedule = GetHours(schedule, currentDay, day);
                currentDay++;
            }

            return schedule;
        }

        //Get all hours
        private Schedule GetHours(Schedule schedule, int currentDay , Day day)
        {
            var hours = _dbAgenda.GetHours(day.Id);

            foreach (var hour in hours)
            {
                schedule.SetCourse(hour.which - 1, currentDay, hour.Course);
                schedule.SetClassName(hour.which - 1, currentDay, hour.Class);
                schedule.SetTeacherName(hour.which - 1, currentDay, hour.ShortName);
            }

            return schedule;
        }

        //Make classroom from hint
        private void NewClassroom(Schedule newSchedule)
        {
            var newRoom = new Classroom();
            newRoom.Id = Guid.NewGuid().ToString();
            newRoom.Name = newSchedule.ClassroomName;

            var classroom = _dbAgenda.GetClassroom(newRoom.Name);
            if (classroom == null)
            {
                _dbAgenda.SaveClassroom(newRoom);
            }
            else
            {
                newRoom.Id = classroom.Id;
            }

            NewYear(newSchedule, newRoom.Id);
        }

        //Make year from hint
        private void NewYear(Schedule newSchedule, string roomId)
        {
            var newYear = new Year();
            newYear.Id = Guid.NewGuid().ToString();
            newYear.SchoolYear = 2018;
            newYear.RoomId = roomId;

            var yearExitst = _dbAgenda.GetYear(newYear.SchoolYear, roomId);

            if (yearExitst == null)
            {
                _dbAgenda.SaveYear(newYear);
            }
            else
            {
                newYear.Id = yearExitst.Id;
            }

            NewPeriod(newSchedule, newYear.Id);
        }

        //Make period from hint
        private void NewPeriod(Schedule newSchedule, string yearId)
        {
            var newPeriod = new Period();
            newPeriod.Id = Guid.NewGuid().ToString();
            newPeriod.PeriodNumber = 4;
            newPeriod.ScheduleYearId = yearId;

            var periodExitst = _dbAgenda.GetPeriode(newPeriod.PeriodNumber, yearId);
            
            if (periodExitst == null)
            {
                _dbAgenda.SavePeriod(newPeriod);
            }
            else
            {
                newPeriod.Id = periodExitst.Id;
            }

            NewWeek(newSchedule, newPeriod.Id);
        }

        //Make week from hint
        private void NewWeek(Schedule newSchedule, string periodId)
        {
            var newWeek = new Week();
            newWeek.Id = Guid.NewGuid().ToString();
            newWeek.WeekNumber = 22;
            newWeek.SchedulePeriodId = periodId;
            newWeek.StartWeek = DateTime.ParseExact(newSchedule.StartDate, "yyyy-MM-dd HH:mm:ss,fff",
                                    CultureInfo.InvariantCulture);
            newWeek.EndWeek = DateTime.ParseExact(newSchedule.EndDate, "yyyy-MM-dd HH:mm:ss,fff",
                                CultureInfo.InvariantCulture);

            var weekExitst = _dbAgenda.GetWeek(newWeek.WeekNumber, periodId);
            if(weekExitst == null)
            {
                _dbAgenda.SaveWeek(newWeek);
            }
            else
            {
                newWeek.Id = weekExitst.Id;
            }

            var dayWeek = newWeek.StartWeek;

            foreach (var currentDay in newSchedule.Days)
            {
                NewDay(currentDay, newWeek.Id,dayWeek);
                dayWeek = dayWeek.AddDays(1);
            }
            
        }

        //Make day from hint
        private void NewDay(MDay currentMDay, string weekId, DateTime dayWeek)
        {
            var newDay = new Day();
            newDay.Id = Guid.NewGuid().ToString();
            newDay.WeekDay = currentMDay.Name;
            newDay.WeekId = weekId;
            newDay.Date = dayWeek;

            var dayExitst = _dbAgenda.GetDay(newDay.WeekDay, weekId);

            if (dayExitst == null)
            {
                _dbAgenda.SaveDay(newDay);
            }
            else
            {
                newDay.Id = dayExitst.Id;
            }

            foreach (var hour in currentMDay.Hours)
            {
                NewHour(hour, newDay.Id);
            }
            
        }

        //Make hour from hint
        private void NewHour(MHour currentMHour, string dayId)
        {
            var newHour = new Hour();
            newHour.Id = Guid.NewGuid().ToString();
            newHour.which = currentMHour.HourId;
            newHour.Class = currentMHour.Class;
            newHour.Course = currentMHour.Course;
            newHour.ShortName = currentMHour.Teacher;
            newHour.ScheduleDayId = dayId;
            newHour.UserId = null;
            _dbAgenda.SaveHour(newHour);
        }
        
        // Remove the zero data
        private Schedule RemoveZeroHours(Schedule schedule)
        {
            var newSchedule = schedule;

            for (var i = 0; i < schedule.Days.Count; i++)
            {
                var removedDay = RemoveZeroHoursDay(schedule.Days[i]);
                newSchedule.Days[i] = removedDay;
            }

            return newSchedule;
        }

        // Remove the zero data
        private MDay RemoveZeroHoursDay(MDay currentDay)
        {
            var newDay = currentDay;
            var totalHours = currentDay.Hours.Count; 
            for (var y = 0; y < totalHours; y++)
            {
                if (!currentDay.Hours[y].Reserved)
                {
                    newDay.Hours.RemoveAt(y);
                    y--;
                    totalHours--;
                }
            }

            return newDay;
        }
    }
}