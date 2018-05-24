using System;

namespace ScheduleCrawler
{
    class Program
    {
        private static string UrlSchool;
        private static string UrlApi;
        private static string Quarter;
        private static string Week;
        private static string Classroom;
        
        private static void Main(string[] args)
        {
            var download = new Download();
            var process = new Process();
            var save = new Save();
            
            var schedulePage = download.GetHTMLPageSchedule();
            var schedule = process.GetModelSchedule(schedulePage);
            schedule.StartDate = "2018-05-21 00:00:00,000";
            schedule.EndDate = "2018-05-27 00:00:00,000";
            
            var done = save.SaveSchedule(schedule);
            
            Console.WriteLine(done);
        }
    }
}