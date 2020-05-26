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
        public const string DbPath = @"database/task_reactor.db";

        /// <summary>
        /// Create the db file and tables if not exists.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized
        [ImportingConstructor]
        public TaskReactorDbContext()
        {
            if(Database is null) throw new NullReferenceException();

            if(!Database.CanConnect())
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DbPath));
                File.Create(DbPath)!.Dispose();
            }

            Database.Migrate();
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

        protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder) => BuildEntity(modelBuilder);

        internal static void BuildEntity([NotNull] ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
            modelBuilder.Entity<TaskDependency>(
                buildAction =>
                {
                    var targetForeignKey = $"{nameof(TaskDependency.Target)}Id";
                    var dependencyForeignKey = $"{nameof(TaskDependency.Dependency)}Id";

                    buildAction!.HasOne(t => t.Target)!.WithMany()!.HasForeignKey(targetForeignKey);
                    buildAction.HasOne(t => t.Dependency)!.WithMany()!.HasForeignKey(dependencyForeignKey);
                    buildAction.HasKey(targetForeignKey, dependencyForeignKey);
                }
            );
        }
    }
}