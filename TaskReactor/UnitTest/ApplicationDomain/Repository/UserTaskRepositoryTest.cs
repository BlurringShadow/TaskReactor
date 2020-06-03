using System;
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
        [NotNull] readonly User _testUser = UserRepositoryTest.TestEntities[0];

        [NotNull, ItemNotNull] internal static UserTask[] TestEntities => new[]
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

        public UserTaskRepositoryTest([NotNull] ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Container.GetExportedValue<IUserRepository>().Register(_testUser);
            Task.WaitAll(Repository.DbSync());
        }

        async Task AddToUser([NotNull] UserTask task)
        {
            Repository.AddToUser(_testUser, task);
            await Repository.DbSync();
            TestOutputHelper.WriteLine(
                $"Successfully add task {JsonSerializer.Serialize(task, SerializerOptions)} " +
                $"\nto user {JsonSerializer.Serialize(_testUser, SerializerOptions)}\n"
            );
        }

        async Task GetAllTest() =>
            TestOutputHelper.WriteLine(
                $"All task from user {JsonSerializer.Serialize(_testUser, SerializerOptions)}:\n" + 
                JsonSerializer.Serialize(await Repository.GetAllFromUserAsync(_testUser), SerializerOptions)
            );

        [Fact]
        async Task Test()
        {
            foreach (var task in TestEntities) await AddToUser(task);
            await GetAllTest();
        }
    }
}