using System;
using ApplicationDomain.Models.Database.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationDomain.Models.Database.Migrations
{
    public partial class InitializeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                $"{nameof(User)}",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(),
                    Password = table.Column<string>()
                },
                constraints: table => { table.PrimaryKey("PK_User", x => x.Id); }
            );

            migrationBuilder.CreateTable(
                $"{nameof(Schedule)}",
                table => new
                {
                    OwnerUserId = table.Column<int>(),
                    Title = table.Column<string>(),
                    StartTime = table.Column<DateTime>($"{SqliteType.Integer}"),
                    DurationOfOneTime = table.Column<TimeSpan>($"{SqliteType.Integer}"),
                    EndTime = table.Column<DateTime>($"{SqliteType.Integer}"),
                    Interval_Kind = table.Column<byte>(nullable: true),
                    Interval_Value = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => new {x.OwnerUserId, x.Title});
                    table.ForeignKey(
                        "FK_Schedule_User_OwnerUserId",
                        x => x.OwnerUserId,
                        "User",
                        "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );
        }
    }
}