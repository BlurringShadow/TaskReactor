using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataRepository;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace UnitTest.ApplicationDomain
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public sealed class UserRepositoryTest : RepositoryTest<User, IUserRepository>
    {
        [NotNull] internal static readonly User[] TestEntities =
        {
            new User
            {
                Name = "first",
                Password = "123"
            },
            new User
            {
                Name = "second",
                Password = "123"
            },
            new User
            {
                Name = "third",
                Password = "123"
            }
        };

        public UserRepositoryTest([NotNull] ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        public static IEnumerable<object[]> GetTestData() => GetTestData(TestEntities);

        [Theory, MemberData(nameof(GetTestData)), Priority(0)]
        public async Task UserRegisterTest([NotNull] User user)
        {
            Repository.Register(user);
            await Repository.DbSync();
            TestOutputHelper.WriteLine(
                $"Successfully register user {JsonSerializer.Serialize(user, SerializerOptions)}"
            );
        }

        [Theory, MemberData(nameof(GetTestData)), Priority(1)]
        public async Task UserLogInTest([NotNull] User user)
        {
            Assert.NotNull(await Repository.LogInAsync(user));
            TestOutputHelper.WriteLine(
                $"Successfully login user {JsonSerializer.Serialize(user, SerializerOptions)}"
            );
        }

        [Theory, MemberData(nameof(GetTestData)), Priority(2)]
        public async Task UserLogOffTest([NotNull] User user)
        {
            Repository.LogOff(user);
            await Repository.DbSync();
            Assert.Null(await Repository.LogInAsync(user));
            TestOutputHelper.WriteLine(
                $"Successfully log off user {JsonSerializer.Serialize(user, SerializerOptions)}"
            );
        }
    }
}