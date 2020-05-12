using System.Configuration;
using ApplicationDomain.Models.DataBase.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.Models.DataBase
{
    public class TaskReactorDbContext : DbContext
    {
        [NotNull] public DbSet<User> Users { get; set; }
        [NotNull] public DbSet<Schedule> Schedules { get; set; }

        // ReSharper disable once NotNullMemberIsNotInitialized
        public TaskReactorDbContext()
        {
        }

        // ReSharper disable once NotNullMemberIsNotInitialized
        public TaskReactorDbContext([NotNull] DbContextOptions<TaskReactorDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring([NotNull] DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder.IsConfigured) return;
            var connections = ConfigurationManager.ConnectionStrings![0];
            optionsBuilder.UseSqlServer(connections!.ConnectionString!);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder!.Entity<Schedule>(
                buildAction =>
                {
                    buildAction!.HasKey(schedule => new {schedule.OwnerUserId, schedule.Title});
                    buildAction.Property(schedule => schedule.StartTime);
                }
            );
        }
    }
}