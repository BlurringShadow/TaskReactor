using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text.Json;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataRepository;
using JetBrains.Annotations;
using Presentation;
using Xunit.Abstractions;

namespace UnitTest.ApplicationDomain.Repository
{
    public abstract class RepositoryTest<TDatabaseModel, TRepository> :
        IRepositoryTest<TDatabaseModel, TRepository>
        where TDatabaseModel : DatabaseModel
        where TRepository : IRepository<TDatabaseModel, TaskReactorDbContext>
    {
        private bool _disposed;

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

        protected RepositoryTest([NotNull] ITestOutputHelper testOutputHelper)
        {
            // Delete the db file before test
            {
                var dbPath = TaskReactorDbContext.DbPath;
                if(Directory.Exists(Path.GetDirectoryName(dbPath)) && File.Exists(dbPath))
                    File.Delete(dbPath);
            }

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

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if(_disposed || !disposing) return;
            Container.Dispose();
            _disposed = true;
        }
    }
}