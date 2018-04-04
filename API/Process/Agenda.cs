using System;
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

        private void NewClassroom(Schedule newSchedule)
        {
            var newRoom = new Classroom();
            newRoom.Id = Guid.NewGuid().ToString();
            newRoom.Name = newSchedule.ClassroomName;
            _dbAgenda.SaveClassroom(newRoom);
            NewYear(newSchedule, newRoom.Id);
        }

        private void NewYear(Schedule newSchedule, string roomId)
        {
            var newYear = new Year();
            newYear.Id = Guid.NewGuid().ToString();
            newYear.SchoolYear = 2018;
            newYear.RoomId = roomId;
            _dbAgenda.SaveYear(newYear);
            
            NewPeriod(newSchedule, newYear.Id);
        }

        private void NewPeriod(Schedule newSchedule, string yearId)
        {
            var newPeriod = new Period();
            newPeriod.Id = Guid.NewGuid().ToString();
            newPeriod.PeriodNumber = 3;
            newPeriod.ScheduleYearId = yearId;
            _dbAgenda.SavePeriod(newPeriod);
            
            NewWeek(newSchedule, newPeriod.Id);
        }

        private void NewWeek(Schedule newSchedule, string periodId)
        {
            var newWeek = new Week();
            newWeek.Id = Guid.NewGuid().ToString();
            newWeek.WeekNumber = 14;
            newWeek.SchedulePeriodId = periodId;
            _dbAgenda.SaveWeek(newWeek);

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
            _dbAgenda.SaveDay(newDay);

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
    }
}