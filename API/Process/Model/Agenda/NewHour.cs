namespace API.Process.Model.Agenda
{
    public class NewHour
    {
        public string Username { get; set; }
        public string Type { get; set; }
        public string Classroom { get; set; }
        public int Quator { get; set; }
        public int Week { get; set; }
        public int StartHour { get; set; }
        public int TotalHours { get; set; }
        public string Day { get; set; }
        public int Year { get; set; }
    }
}