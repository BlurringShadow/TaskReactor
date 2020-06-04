using System;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace ApplicationDomain.Database.Migration
{
    [DbContext(typeof(TaskReactorDbContext)),
     Migration(nameof(Initialization)),
     System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Initialization : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder) =>
            TaskReactorDbContext.BuildEntity(modelBuilder);

        class UserTable
        {
            [NotNull] public OperationBuilder<AddColumnOperation> Id { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> Name { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> Password { get; }

            public UserTable([NotNull] ColumnsBuilder builder)
            {
                Id = builder.Column<int>(nullable: false)!
                    .Annotation("Sqlite:Autoincrement", true)!;
                Name = builder.Column<string>(nullable: false)!;
                Password = builder.Column<string>(nullable: false)!;
            }
        }

        class UserTaskTable
        {
            [NotNull] public OperationBuilder<AddColumnOperation> Id { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> Title { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> Description { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> StartTime { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> EndTime { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> OwnerUserId { get; }

            public UserTaskTable([NotNull] ColumnsBuilder builder)
            {
                Id = builder.Column<int>(nullable: false)!
                    .Annotation("Sqlite:Autoincrement", true)!;
                Title = builder.Column<string>(nullable: false)!;
                Description = builder.Column<string>(nullable: true)!;
                StartTime = builder.Column<DateTime>(nameof(SqliteType.Integer), nullable: false)!;
                EndTime = builder.Column<DateTime>(nameof(SqliteType.Integer), nullable: false)!;
                OwnerUserId = builder.Column<int>(nullable: false)!;
            }
        }

        class GoalTable
        {
            [NotNull] public OperationBuilder<AddColumnOperation> Id { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> Title { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> Description { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> StartTime { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> EndTime { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> DurationOfOneTime { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> Interval_Kind { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> Interval_Value { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> FromTaskId { get; }

            public GoalTable([NotNull] ColumnsBuilder table)
            {
                Id = table.Column<int>(nullable: false)!
                    .Annotation("Sqlite:Autoincrement", true)!;
                Title = table.Column<string>(nullable: false)!;
                Description = table.Column<string>(nullable: true)!;
                StartTime = table.Column<DateTime>(
                    nameof(SqliteType.Integer),
                    false
                )!;
                EndTime = table.Column<DateTime>(
                    nameof(SqliteType.Integer),
                    false
                )!;
                DurationOfOneTime = table.Column<TimeSpan>(
                    nameof(SqliteType.Integer),
                    false
                )!;
                Interval_Kind = table.Column<byte>(nullable: true)!;
                Interval_Value = table.Column<byte>(nullable: true)!;
                FromTaskId = table.Column<int>(nullable: false)!;
            }
        }

        class TaskDependencyTable
        {
            [NotNull] public OperationBuilder<AddColumnOperation> TargetId { get; }
            [NotNull] public OperationBuilder<AddColumnOperation> DependencyId { get; }

            public TaskDependencyTable([NotNull] ColumnsBuilder builder)
            {
                TargetId = builder.Column<int>(nullable: false)!;
                DependencyId = builder.Column<int>(nullable: false)!;
            }

            public TaskDependencyTable(
                [NotNull] OperationBuilder<AddColumnOperation> targetId,
                [NotNull] OperationBuilder<AddColumnOperation> dependencyId
            )
            {
                TargetId = targetId;
                DependencyId = dependencyId;
            }
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                nameof(User),
                table => new UserTable(table!),
                constraints: table => table!.PrimaryKey($"PK_{nameof(User)}", x => x.Id)
            );

            migrationBuilder.CreateTable(
                nameof(UserTask),
                table => new UserTaskTable(table!),
                constraints: table =>
                {
                    table!.PrimaryKey($"PK_{nameof(UserTask)}", x => x.Id);
                    table.ForeignKey(
                        $"FK_{nameof(UserTask)}_{nameof(User)}_{nameof(UserTask.OwnerUser)}Id",
                        x => x.OwnerUserId,
                        nameof(User),
                        "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                nameof(Goal),
                table => new GoalTable(table!)
                ,
                constraints: table =>
                {
                    table!.PrimaryKey($"PK_{nameof(Goal)}", x => x.Id);
                    table.ForeignKey(
                        $"FK_{nameof(Goal)}_{nameof(UserTask)}_{nameof(Goal.FromTask)}Id",
                        x => x.FromTaskId,
                        nameof(UserTask),
                        "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                nameof(TaskDependency),
                table => new TaskDependencyTable(table!),
                constraints: table =>
                {
                    table!.PrimaryKey(
                        $"PK_{nameof(TaskDependency)}", x => new TaskDependencyTable(x.TargetId, x.DependencyId)
                    );
                    table.ForeignKey(
                        $"FK_{nameof(TaskDependency)}_{nameof(UserTask)}_{nameof(TaskDependency.Dependency)}Id",
                        x => x.DependencyId,
                        nameof(UserTask),
                        "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        $"FK_{nameof(TaskDependency)}_{nameof(UserTask)}_{nameof(TaskDependency.Target)}Id",
                        x => x.TargetId,
                        nameof(UserTask),
                        "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
        }
    }
}