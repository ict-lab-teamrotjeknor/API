using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Design;

namespace API.Models.Data.Query
{
    public class DbAgenda
    {
        private ApplicationDbContext _dbContext;

        public DbAgenda(ApplicationDbContext newDbContext)
        {
            _dbContext = newDbContext;
        }

        public Classroom GetClassroom(string classroomName)
        {
            var classroom = _dbContext.Classrooms
                .SingleOrDefault(c => c.Name.Equals(classroomName));

            return classroom;
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

            }
        }

        public List<Hour> GetHours(string dayId)
        {
            var hours = _dbContext.Hours
                .Where(h => h.ScheduleDayId.Equals(dayId)).ToList();

            return hours;
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

            }
        }
    }
}