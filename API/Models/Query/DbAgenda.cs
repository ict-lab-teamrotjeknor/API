using System;
using System.Collections.Generic;
using System.Linq;
using API.Process.Model.Agenda;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;


namespace API.Models.Data.Query
{
    public class DbAgenda : IDbAgenda
    {
        private ApplicationDbContext _dbContext;
        private ILogger _logger;

        public DbAgenda(ApplicationDbContext newDbContext, ILogger logger)
        {
            _logger = logger;
            _dbContext = newDbContext;
        }

        public Classroom GetClassroom(string classroomName)
        {
            try
            {
                var classroom = _dbContext.Classrooms
                    .SingleOrDefault(c => c.Name.Equals(classroomName));
                return classroom;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return new Classroom();
        }

        public void SaveClassroom(Classroom newRoom)
        {
            try
            {
                _dbContext.Classrooms.Add(newRoom);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public Year GetYear(int year, string roomId)
        {
            var yearExitst = _dbContext.Years
                .SingleOrDefault(y => y.SchoolYear.Equals(year) && y.RoomId.Equals(roomId));

            return yearExitst;
        }

        public void SaveYear(Year newYear)
        {
            try
            {
                _dbContext.Years.Add(newYear);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public Period GetPeriode(int periode, string yearId)
        {
            var periodExitst = _dbContext.Periods
                .SingleOrDefault(p => p.PeriodNumber.Equals(periode) && p.ScheduleYearId.Equals(yearId));

            return periodExitst;
        }

        public void SavePeriod(Period newPeriod)
        {
            try
            {
                _dbContext.Periods.Add(newPeriod);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public Week GetWeek(int week, string periodId)
        {
            var weekExitst = _dbContext.Weeks
                .SingleOrDefault(w => w.WeekNumber.Equals(week) && w.SchedulePeriodId.Equals(periodId));

            return weekExitst;
        }
        
        public void SaveWeek(Week newWeek)
        {
            try
            {
                _dbContext.Weeks.Add(newWeek);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public Day GetDay(string day, string weekId)
        {
            var dayExitst = _dbContext.Days
                .SingleOrDefault(d => d.WeekDay.Equals(day) && d.WeekId.Equals(weekId));
            
            return dayExitst;
        }

        public List<Day> GetDays(string weekId)
        {
            var days = _dbContext.Days
                .Where(d => d.WeekId.Equals(weekId)).ToList();

            return days;
        }
        
        public void SaveDay(Day newDay)
        {
            try
            {
                _dbContext.Days.Add(newDay);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public List<Hour> GetHours(string dayId)
        {
            var hours = _dbContext.Hours
                .Where(h => h.ScheduleDayId.Equals(dayId)).ToList();

            return hours;
        }

        public string FindHours(NewHour newHour)
        {
            var dayId = GetIdDay(newHour);
            
            for (var i = 0; i < newHour.TotalHours; i++) {
                var hoursExists = _dbContext.Classrooms
                    .Where(c => c.Name.Equals(newHour.Classroom))
                    .Join(_dbContext.Years,
                        c => c.Id,
                        y => y.RoomId,
                        (c, y) => new {Classroom = c, Year = y})
                    .Where(y => y.Year.SchoolYear.Equals(newHour.Year))
                    .Join(_dbContext.Periods,
                        y => y.Year.Id,
                        p => p.ScheduleYearId,
                        (y, p) => new {Year = y, Period = p})
                    .Where(p => p.Period.PeriodNumber.Equals(newHour.Quator))
                    .Join(_dbContext.Weeks,
                        p => p.Period.Id,
                        w => w.SchedulePeriodId,    
                        (p, w) => new {Period = p, Week = w})
                    .Where(w => w.Week.WeekNumber.Equals(newHour.Week))
                    .Join(_dbContext.Days,
                        w => w.Week.Id,
                        d => d.WeekId,
                        (w, d) => new {Week = w, Day = d})
                    .Where(d => d.Day.WeekDay.Equals(newHour.Day))
                    .Join(_dbContext.Hours,
                        d => d.Day.Id,
                        h => h.ScheduleDayId,
                        (d, h) => new {Day = d, Hour = h})
                    .Where(h => h.Hour.which.Equals(newHour.StartHour))
                    .ToList();
                
                if (hoursExists.Count > 0)
                {
                    return string.Empty;
                }
            }

            return dayId;
        }

        public List<string> GetReservations(string userId)
        {
            var currentUser = _dbContext.Users
                .SingleOrDefault(u => u.Id.Equals(userId));

            var days = _dbContext.Days
                .Join(_dbContext.Hours,
                    d => d.Id,
                    h => h.ScheduleDayId,
                    (d, h) => new {Day = d, Hour = h})
                .Where(dh => dh.Hour.ShortName.Equals(currentUser.Email))
                .ToList();

            var allDays = new List<string>();
            foreach (var day in days)
            {
                allDays.Add(day.Day.Date.ToString());
            }

            allDays = allDays.Distinct().ToList();
            return allDays;
        }

        public MDay GetReservations(string userId, DateTime Day)
        {
            var currentUser = _dbContext.Users
                .SingleOrDefault(u => u.Id.Equals(userId));
            
            var days = _dbContext.Days
                .Where(d => d.Date.Equals(Day))
                .Join(_dbContext.Hours,
                    d => d.Id,
                    h => h.ScheduleDayId,
                    (d, h) => new {Day = d, Hour = h})
                .Where(dh => dh.Hour.ShortName.Equals(currentUser.Email))
                .ToList();

            var setDay = new MDay(days[0].Day.WeekDay);

            foreach (var day in days)
            {
                var tempHour = day.Hour;
                
                var newHour = new MHour(day.Hour.which);
                newHour.Class = tempHour.Class;
                newHour.Course = tempHour.Course;
                newHour.Teacher = tempHour.ShortName;
                newHour.Reserved = true;
                setDay.Hours.Add(newHour);
            }

            return setDay;    
        }

        public void SaveHour(Hour newHour)
        {
            try
            {
                _dbContext.Hours.Add(newHour);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private string GetIdDay(NewHour newHour)
        {
            var hoursExists = _dbContext.Classrooms
                .Where(c => c.Name.Equals(newHour.Classroom))
                .Join(_dbContext.Years,
                    c => c.Id,
                    y => y.RoomId,
                    (c, y) => new {Classroom = c, Year = y})
                .Where(y => y.Year.SchoolYear.Equals(newHour.Year))
                .Join(_dbContext.Periods,
                    y => y.Year.Id,
                    p => p.ScheduleYearId,
                    (y, p) => new {Year = y, Period = p})
                .Where(p => p.Period.PeriodNumber.Equals(newHour.Quator))
                .Join(_dbContext.Weeks,
                    p => p.Period.Id,
                    w => w.SchedulePeriodId,
                    (p, w) => new {Period = p, Week = w})
                .Where(w => w.Week.WeekNumber.Equals(newHour.Week))
                .Join(_dbContext.Days,
                    w => w.Week.Id,
                    d => d.WeekId,
                    (w, d) => new {Week = w, Day = d})
                .Where(d => d.Day.WeekDay.Equals(newHour.Day)).ToList();
            return hoursExists[0].Day.Id;
        }
    }
}