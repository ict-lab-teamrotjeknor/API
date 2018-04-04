namespace API.Process.Model.Agenda
{
    public class MHour
    {
        public int HourId { get; set; }
        public string Class { get; set; }
        public string Teacher { get; set; }
        public string Course { get; set; }
        public string SpecialReason { get; set; }
        public bool Reserved { get; set; } = false;

        public MHour(int hourId)
        {
            this.HourId = hourId;
        }
    }
}