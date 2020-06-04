using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataRepository;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace UnitTest.ApplicationDomain.Repository
{
    public abstract class RepositoryTest<TDatabaseModel, TRepository> :
        RepositoryTestBase, IRepositoryTest<TDatabaseModel, TRepository, TaskReactorDbContext>
        where TDatabaseModel : DatabaseModel
        where TRepository : IRepository<TDatabaseModel, TaskReactorDbContext>
    {
        /// <summary>
        /// Output message helper <see cref="ITestOutputHelper"/>
        /// </summary>
        [NotNull] public ITestOutputHelper TestOutputHelper { get; }

        [NotNull] public JsonSerializerOptions SerializerOptions { get; }

        [NotNull] public TRepository Repository { get; }

        protected static IEnumerable<object[]> GetTestData(
            [NotNull, ItemNotNull] IEnumerable<TDatabaseModel> testEntities
        ) => testEntities.Select(entity => new[] {entity}).ToList();

        protected RepositoryTest([NotNull] ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
            Repository = Container.GetExportedValue<TRepository>();
            SerializerOptions = new JsonSerializerOptions {WriteIndented = true};
        }
    }
}