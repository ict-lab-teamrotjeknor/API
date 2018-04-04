using System;
using System.Collections.Generic;

namespace API.Models.Data
{
    public class Day
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string WeekDay { get; set; }
        public string WeekId { get; set; }

        public List<Hour> Hours { get; set; }
        public Week Week { get; set; }
    }
}