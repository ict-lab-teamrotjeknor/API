using System;
using System.Globalization;
using System.Net.Mime;
using System.Threading.Tasks.Dataflow;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process.Model;
using API.Process.Model.Agenda;
using Newtonsoft.Json.Linq;
using Day = API.Models.Data.Day;
using Hour = API.Models.Data.Hour;

namespace API.Process
{
    public class Agenda
    {
        private DbAgenda _dbAgenda;
        private JsonEditor _jsonEditor;

        public Agenda(ApplicationDbContext dbContext)
        {
            _dbAgenda = new DbAgenda(dbContext);
            _jsonEditor = new JsonEditor();
        }
        
        public JObject Upload(JObject newWeek)
        {
            var schedule = _jsonEditor.GetSchedule(newWeek);
            NewClassroom(schedule);

            return new JObject();
        }

        public JObject GetWeek(string roomName, int year, int kwartaal, int weekNumber)
        {
            var classroom = _dbAgenda.GetClassroom(roomName);
            var yearModel = _dbAgenda.GetYear(year, classroom.Id);
            var kwartaalModel = _dbAgenda.GetPeriode(kwartaal, yearModel.Id);
            var week = _dbAgenda.GetWeek(weekNumber, kwartaalModel.Id);
            
            var schedule = new Schedule();
            schedule.ClassroomName = classroom.Name;
            schedule.StartDate = week.StartWeek.ToString(CultureInfo.CurrentCulture);
            schedule.EndDate = week.EndWeek.ToString(CultureInfo.CurrentCulture);
            schedule = GetDays(schedule, week);
            schedule = RemoveZeroHours(schedule);

            return _jsonEditor.SerilizeJObject(schedule);
        }

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

        private Schedule GetHours(Schedule schedule, int currentDay , Day day)
        {
            var hours = _dbAgenda.GetHours(day.Id);

            foreach (var hour in hours)
            {
                schedule.SetCourse(hour.which, currentDay, hour.Course);
                schedule.SetClassName(hour.which, currentDay, hour.Class);
                schedule.SetTeacherName(hour.which, currentDay, hour.ShortName);
            }

            return schedule;
        }

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

        private void NewWeek(Schedule newSchedule, string periodId)
        {
            var newWeek = new Week();
            newWeek.Id = Guid.NewGuid().ToString();
            newWeek.WeekNumber = 20;
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

            foreach (var currentDay in newSchedule.Days)
            {
                NewDay(currentDay, newWeek.Id);
            }
            
        }

        private void NewDay(MDay currentMDay, string weekId)
        {
            var newDay = new Day();
            newDay.Id = Guid.NewGuid().ToString();
            newDay.WeekDay = currentMDay.Name;
            newDay.WeekId = weekId;

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
        
        private Schedule RemoveZeroHours(Schedule schedule)
        {
            var newSchedule = schedule;

            for (var i = 0; i < schedule.Days.Count; i++)
            {
                var totalHours = schedule.Days[i].Hours.Count; 
                for (var y = 0; y < totalHours; y++)
                {
                    if (!schedule.Days[i].Hours[y].Reserved)
                    {
                        newSchedule.Days[i].Hours.RemoveAt(y);
                        y--;
                        totalHours--;
                    }
                }
            }

            return newSchedule;
        }
    }
}