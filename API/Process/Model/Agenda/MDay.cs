using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace API.Process.Model.Agenda
{
    public class MDay
    {
        public string Name { get; private set;  }
        public List<MHour> Hours { get; set; }

        public MDay(string name)
        {
            this.Hours = new List<MHour>();
            this.Name = name;
            CreateHours();
        }

        private void CreateHours()
        {
            for (int i = 1; i < 16; i++)
            {
                this.Hours.Add(new MHour(i));
            }
        }
    }
}