using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace ScheduleCrawler.Models
{
    public class Day
    {
        public string Name { get; private set;  }
        public List<Hour> Hours { get; set; }

        public Day(string name)
        {
            this.Hours = new List<Hour>();
            this.Name = name;
            CreateHours();
        }

        private void CreateHours()
        {
            for (int i = 1; i < 16; i++)
            {
                this.Hours.Add(new Hour(i));
            }
        }
    }
}