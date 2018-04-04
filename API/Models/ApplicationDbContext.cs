using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Data
{
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
        }

        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Hour> Hours { get; set; }
        public DbSet<Period> Periods { get; set;}
        public DbSet<Week> Weeks { get; set; }
        public DbSet<Year> Years { get; set; }

    }
}