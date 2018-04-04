using System;

namespace API.Models.Data.Query
{
    public class DbAgenda
    {
        private ApplicationDbContext _dbContext;

        public DbAgenda(ApplicationDbContext newDbContext)
        {
            _dbContext = newDbContext;
        }

        public Classroom GetWeek()
        {
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

            }
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