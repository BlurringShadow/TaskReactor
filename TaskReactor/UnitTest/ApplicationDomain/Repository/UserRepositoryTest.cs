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
    public sealed class UserRepositoryTest : RepositoryTest<User, IUserRepository>
    {
        [NotNull, ItemNotNull] internal static readonly User[] TestEntities =
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

        public static IEnumerable<object[]> GetTestData() => GetTestData(TestEntities);

        public UserRepositoryTest([NotNull] ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        async Task UserRegisterTest([NotNull] User user)
        {
            Repository.Register(user);
            await Repository.DbSync();
            TestOutputHelper.WriteLine(
                $"Successfully register user {JsonSerializer.Serialize(user, SerializerOptions)}"
            );
        }

        async Task UserLogInTest([NotNull] User user)
        {
            Assert.NotNull(await Repository.LogInAsync(user));
            TestOutputHelper.WriteLine(
                $"Successfully login user {JsonSerializer.Serialize(user, SerializerOptions)}"
            );
        }

        async Task UserLogOffTest([NotNull] User user)
        {
            Repository.LogOff(user);
            await Repository.DbSync();
            Assert.Null(await Repository.LogInAsync(user));
            TestOutputHelper.WriteLine(
                $"Successfully log off user {JsonSerializer.Serialize(user, SerializerOptions)}"
            );
        }

        [Theory, MemberData(nameof(GetTestData))]
        async Task Test([NotNull] User user)
        {
            await UserRegisterTest(user);
            await UserLogInTest(user);
            await UserLogOffTest(user);
        }
    }
}