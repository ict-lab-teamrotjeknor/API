using System.Collections.Generic;

namespace API.Models.Data.Query
{
    public interface IDbAgenda
    {
        Classroom GetClassroom(string classroomName);
        void SaveClassroom(Classroom newRoom);
        Year GetYear(int year, string roomId);
        void SaveYear(Year newYear);
        Period GetPeriode(int periode, string yearId);
        void SavePeriod(Period newPeriod);
        Week GetWeek(int week, string periodId);
        void SaveWeek(Week newWeek);
        Day GetDay(string day, string weekId);
        List<Day> GetDays(string weekId);
        void SaveDay(Day newDay);
        List<Hour> GetHours(string dayId);
        void SaveHour(Hour newHour);
    }
}