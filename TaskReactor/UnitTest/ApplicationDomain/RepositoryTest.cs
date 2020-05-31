using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text.Json;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.Repository;
using JetBrains.Annotations;
using Presentation;
using Xunit.Abstractions;

namespace UnitTest.ApplicationDomain
{
    public abstract class RepositoryTest<TDatabaseModel, TRepository> : IRepositoryTest<TDatabaseModel, TRepository>
        where TDatabaseModel : DatabaseModel
        where TRepository : Repository<TDatabaseModel, TaskReactorDbContext>
    {
        /// <summary>
        /// Output message helper <see cref="ITestOutputHelper"/>
        /// </summary>
        [NotNull] public ITestOutputHelper TestOutputHelper { get; }

        [NotNull] public CompositionContainer Container { get; }

        [NotNull] public JsonSerializerOptions SerializerOptions { get; }

        [NotNull] public TRepository Repository { get; }

        protected static IEnumerable<object[]> GetTestData(
            [NotNull, ItemNotNull] IEnumerable<TDatabaseModel> testEntities
        ) => testEntities.Select(entity => new[] {entity}).ToList();

        static RepositoryTest()
        {
            // Delete the db file before test
            if(Directory.Exists(Path.GetDirectoryName(TaskReactorDbContext.DbPath)) &&
               File.Exists(TaskReactorDbContext.DbPath))
                File.Delete(TaskReactorDbContext.DbPath);
        }

        protected RepositoryTest([NotNull] ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;

            Container = GetContainer();
            Repository = Container.GetExportedValue<TRepository>();
            SerializerOptions = new JsonSerializerOptions {WriteIndented = true};
        }

        [NotNull]
        static CompositionContainer GetContainer() =>
            new CompositionContainer(
                new AggregateCatalog(
                    new AssemblyCatalog(typeof(IDatabaseModel).Assembly!),
                    new AssemblyCatalog(typeof(App).Assembly!)
                )
            );
    }
}