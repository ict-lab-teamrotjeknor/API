using System.Collections.Generic;
using API.Models.Data;
using API.Models.Data.Query;

namespace UnitTest
{
    public class DbAgendaFake : IDbAgenda
    {
        public Classroom GetClassroom(string classroomName)
        {
            throw new System.NotImplementedException();
        }

        public void SaveClassroom(Classroom newRoom)
        {
            throw new System.NotImplementedException();
        }

        public Year GetYear(int year, string roomId)
        {
            throw new System.NotImplementedException();
        }

        public void SaveYear(Year newYear)
        {
            throw new System.NotImplementedException();
        }

        public Period GetPeriode(int periode, string yearId)
        {
            throw new System.NotImplementedException();
        }

        public void SavePeriod(Period newPeriod)
        {
            throw new System.NotImplementedException();
        }

        public Week GetWeek(int week, string periodId)
        {
            throw new System.NotImplementedException();
        }

        public void SaveWeek(Week newWeek)
        {
            throw new System.NotImplementedException();
        }

        public Day GetDay(string day, string weekId)
        {
            throw new System.NotImplementedException();
        }

        public List<Day> GetDays(string weekId)
        {
            throw new System.NotImplementedException();
        }

        public void SaveDay(Day newDay)
        {
            throw new System.NotImplementedException();
        }

        public List<Hour> GetHours(string dayId)
        {
            throw new System.NotImplementedException();
        }

        public void SaveHour(Hour newHour)
        {
            throw new System.NotImplementedException();
        }
    }
}