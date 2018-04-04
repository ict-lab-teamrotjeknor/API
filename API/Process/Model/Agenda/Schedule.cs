using System.Collections.Generic;

namespace API.Process.Model.Agenda
{
    public class Schedule
    {
        public string ClassroomName { get; set; }
        public string ClassroomType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SchoolType { get; set; }
        public List<MDay> Days { get; set; }

        public Schedule()
        {
            this.Days = new List<MDay>();
        }

        public void AddDay(string dayName)
        {
            Days.Add(new MDay(dayName));
        }

        public void SetClassName(int hour, int day, string name)
        {
            Days[day].Hours[hour].Class = name;
            Days[day].Hours[hour].Reserved = true;
        }

        public void SetTeacherName(int hour, int day, string name)
        {
            Days[day].Hours[hour].Teacher = name;
        }

        public void SetCourse(int hour, int day, string name)
        {
            Days[day].Hours[hour].Course = name;
        }

        public bool CheckHourExists(int hour, int day)
        {
            return Days[day].Hours[hour].Reserved;
        }
        
        public void SetSpecialReason(int hour, int day, string reason)
        {
            Days[day].Hours[hour].SpecialReason = reason;
            Days[day].Hours[hour].Reserved = true;
        }
    }
}