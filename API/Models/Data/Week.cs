using System;
using System.Collections.Generic;

namespace API.Models.Data
{
    public class Week
    {
        public string Id { get; set; }
        public int WeekNumber { get; set; }
        public DateTime StartWeek { get; set; }
        public DateTime EndWeek { get; set; }
        public string SchedulePeriodId { get; set; }

        public List<Day> Days { get; set; }
        public Period Period { get; set; }

    }
}