namespace API.Models.Data
{
    public class Hour
    {
        public string Id { get; set; }
        public int which { get; set; }
        public string Class { get; set; }
        public string Course { get; set; }
        public string ShortName { get; set; }
        public string UserId { get; set; }
        public string ScheduleDayId { get; set; }

        public User User { get; set; }
        public Day Day { get; set; }
    }
}