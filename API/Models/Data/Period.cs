using System.Collections.Generic;

namespace API.Models.Data
{
    public class Period
    {
        public string Id { get; set; }
        public int PeriodNumber { get; set; }
        public string ScheduleYearId { get; set; }

        public List<Week> Week { get; set; }
        public Year Year { get; set; }
    }
}