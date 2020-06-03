using System.Text.Json;
using System.Threading.Tasks;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataRepository;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ApplicationDomain.Repository
{
    public sealed class TaskDependenciesRepositoryTest : RepositoryTest<TaskDependency, ITaskDependencyRepository>
    {
        [NotNull] readonly User _testUser = UserRepositoryTest.TestEntities[0];

        public TaskDependenciesRepositoryTest([NotNull] ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var userRepository = Container.GetExportedValue<IUserRepository>();

            _testUser.Tasks = UserTaskRepositoryTest.TestEntities;

            userRepository.Register(_testUser);

            Task.WaitAll(userRepository.DbSync());
        }

        async Task AddDependenciesTest(int targetIndex, int dependencyIndex)
        {
            var dependencies =
                Repository.AddDependencies(_testUser.Tasks![targetIndex], _testUser.Tasks[dependencyIndex]);
            await Repository.DbSync();
            TestOutputHelper.WriteLine(
                $"Successfully add task dependency {JsonSerializer.Serialize(dependencies, SerializerOptions)}"
            );
        }

        async Task GetDependenciesTest([NotNull] UserTask task) =>
            TestOutputHelper.WriteLine(
                $"\nTask {JsonSerializer.Serialize(task, SerializerOptions)}\nhas follow dependencies:\n" +
                JsonSerializer.Serialize(await Repository.GetDependenciesAsync(task), SerializerOptions)
            );

        [Fact]
        async Task Test()
        {
            await AddDependenciesTest(1, 0);
            await AddDependenciesTest(2, 1);

            foreach (var task in _testUser.Tasks!)
                await GetDependenciesTest(task);
        }
    }
}