using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Data
{
    //Structure of the database
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hour>()
                .HasOne(u => u.User)
                .WithMany(h => h.Hours)
                .HasForeignKey(h => h.UserId);

            modelBuilder.Entity<Hour>()
                .HasOne(d => d.Day)
                .WithMany(h => h.Hours)
                .HasForeignKey(h => h.ScheduleDayId);

            modelBuilder.Entity<Day>()
                .HasOne(w => w.Week)
                .WithMany(d => d.Days)
                .HasForeignKey(d => d.WeekId);

            modelBuilder.Entity<Week>()
                .HasOne(p => p.Period)
                .WithMany(w => w.Week)
                .HasForeignKey(w => w.SchedulePeriodId);

            modelBuilder.Entity<Period>()
                .HasOne(y => y.Year)
                .WithMany(p => p.Periods)
                .HasForeignKey(p => p.ScheduleYearId);

            modelBuilder.Entity<Year>()
                .HasOne(b => b.Classroom)
                .WithMany(y => y.Years)
                .HasForeignKey(y => y.RoomId);

            modelBuilder.Entity<Sensor>()
                .HasOne(b => b.Classroom)
                .WithMany(s => s.Sensors)
                .HasForeignKey(s => s.RoomId);

            modelBuilder.Entity<SensorData>()
                .HasOne(s => s.Sensor)
                .WithMany(d => d.SensorDatas)
                .HasForeignKey(d => d.SensorId);

            modelBuilder.Entity<Classroom>()
                .HasOne(p => p.Pi)
                .WithOne(c => c.Classroom)
                .HasForeignKey<Classroom>(c => c.PiID);
        }

        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Hour> Hours { get; set; }
        public DbSet<Period> Periods { get; set;}
        public DbSet<Week> Weeks { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorData> SensorDatas { get; set; }
        public DbSet<PI> PI { get; set; }
    }
}