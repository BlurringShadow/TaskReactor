using System.ComponentModel.Composition.Hosting;
using System.Text.Json;
using ApplicationDomain.Models.Database;
using ApplicationDomain.Models.Database.Entity;
using ApplicationDomain.Repository;
using Xunit.Abstractions;

namespace UnitTest.ApplicationDomain
{
    interface IRepositoryTest<TDatabaseModel, out TRepository>
        where TDatabaseModel : DatabaseModel 
        where TRepository : Repository<TDatabaseModel, TaskReactorDbContext>
    {
        ITestOutputHelper TestOutputHelper { get; }
        CompositionContainer Container { get; }
        JsonSerializerOptions SerializerOptions { get; }
        TRepository Repository { get; }
    }
}