using System;
using System.ComponentModel.Composition;
using System.IO;
using ApplicationDomain.Models.Database.Entity;
using JetBrains.Annotations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.Models.Database
{
    [Export]
    public sealed class TaskReactorDbContext : DbContext
    {
        public const string DbPath = @"DataBase/TaskReactor.db";

        // ReSharper disable once NotNullMemberIsNotInitialized
        public TaskReactorDbContext() : this(new DbContextOptions<TaskReactorDbContext>())
        {
        }

        /// <summary>
        /// Create the db file and tables if not exists.
        /// </summary>
        /// <param name="options"> <see cref="DbContext(DbContextOptions)"/>> </param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public TaskReactorDbContext([NotNull] DbContextOptions<TaskReactorDbContext> options) : base(options)
        {
            if(Database is null) throw new NullReferenceException();

            if(Database.CanConnect()) Database.Migrate();
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DbPath));
                File.Create(DbPath)!.Dispose();
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring([NotNull] DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder.IsConfigured) return;

            optionsBuilder.UseSqlite(
                new SqliteConnectionStringBuilder
                {
                    DataSource = DbPath,
                    Mode = SqliteOpenMode.ReadWriteCreate
                }.ConnectionString
            );
        }

        protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();

            // configure composition key
            modelBuilder.Entity<Schedule>(
                buildAction => buildAction!.HasKey(schedule => new {schedule.OwnerUserId, schedule.Title})
            );
        }
    }
}