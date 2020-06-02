using System.ComponentModel.Composition.Hosting;
using System.Text.Json;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataRepository;
using Xunit.Abstractions;

namespace UnitTest.ApplicationDomain
{
    interface IRepositoryTest<TDatabaseModel, out TRepository>
        where TDatabaseModel : DatabaseModel 
        where TRepository : IRepository<TDatabaseModel, TaskReactorDbContext>
    {
        ITestOutputHelper TestOutputHelper { get; }
        CompositionContainer Container { get; }
        JsonSerializerOptions SerializerOptions { get; }
        TRepository Repository { get; }
    }
}