using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataRepository;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ApplicationDomain.Repository
{
    public sealed class UserTaskRepositoryTest : RepositoryTest<UserTask, IUserTaskRepository>
    {
        [NotNull] internal static readonly User TestUser = UserRepositoryTest.TestEntities[0];

        [NotNull, ItemNotNull] internal static readonly UserTask[] TestEntities =
        {
            new UserTask
            {
                Title = "Finish Homework",
                Description = "Cooperation",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now + TimeSpan.FromDays(2)
            },
            new UserTask
            {
                Title = "Touch fish",
                StartTime = DateTime.Now,
                Description = "Template meta programming",
                EndTime = DateTime.Now + TimeSpan.FromDays(3)
            },
            new UserTask
            {
                Title = "Publish new projects ",
                StartTime = DateTime.Now,
                Description = "Dump them halfway",
                EndTime = DateTime.Now + TimeSpan.FromDays(4)
            }
        };

        public static IEnumerable<object[]> GetTestData() => GetTestData(TestEntities);

        public UserTaskRepositoryTest([NotNull] ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Container.GetExportedValue<IUserRepository>().Register(TestUser);
            Task.WaitAll(Repository.DbSync());
        }

        async Task AddToUser([NotNull] UserTask task)
        {
            Repository.AddToUser(TestUser, task);
            await Repository.DbSync();
            TestOutputHelper.WriteLine(
                $"Successfully add task {JsonSerializer.Serialize(task, SerializerOptions)} " +
                $"\nto user {JsonSerializer.Serialize(TestUser, SerializerOptions)}\n"
            );
        }

        async Task GetAllTest() =>
            TestOutputHelper.WriteLine(
                $"All task from user {JsonSerializer.Serialize(TestUser, SerializerOptions)}:\n" + 
                JsonSerializer.Serialize(await Repository.GetAllFromUserAsync(TestUser), SerializerOptions)
            );

        [Fact]
        async Task Test()
        {
            foreach (var task in TestEntities) await AddToUser(task);
            await GetAllTest();
        }
    }
}