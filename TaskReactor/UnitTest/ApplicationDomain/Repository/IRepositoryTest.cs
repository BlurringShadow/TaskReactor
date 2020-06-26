using System.Text.Json;
using Data.Database.Entity;
using Data.DataRepository;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace UnitTest.ApplicationDomain.Repository
{
    interface IRepositoryTest<TDatabaseModel, out TRepository, TDbContext>
        where TDatabaseModel : DatabaseModel
        where TRepository : IRepository<TDatabaseModel, TDbContext>
        where TDbContext : DbContext
    {
        ITestOutputHelper TestOutputHelper { get; }
        JsonSerializerOptions SerializerOptions { get; }
        TRepository Repository { get; }
    }
}