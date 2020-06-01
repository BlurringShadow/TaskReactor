using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataModel;
using ApplicationDomain.DataRepository;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.ModelService
{
    public abstract class Service<TDatabaseModel, TDbContext, TRepository, TModel>
        where TDatabaseModel : DatabaseModel, new()
        where TDbContext : DbContext
        where TRepository : IRepository<TDatabaseModel, TDbContext>
        where TModel : Model<TDatabaseModel>
    {
        [NotNull] public TRepository Repository { get; }

        [NotNull] public TDbContext Context => Repository.Context;

        [NotNull] public DbSet<TDatabaseModel> DbSet => Repository.DbSet;

        protected Service([NotNull] TRepository repository) => Repository = repository;

        [NotNull]
        public async Task<bool> ContainsByKeyAsync(IEnumerable keys) =>
            await ContainsByKeyAsync(keys, CancellationToken.None);

        [NotNull]
        public async Task<bool> ContainsByKeyAsync(IEnumerable keys, CancellationToken token) =>
            await Repository.ContainsByKeyAsync(keys, token);
    }
}