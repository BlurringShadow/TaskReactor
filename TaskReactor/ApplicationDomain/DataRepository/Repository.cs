using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.DataRepository
{
    public abstract class Repository<TDatabaseModel, TDbContext> : IRepository<TDatabaseModel, TDbContext>
        where TDbContext : DbContext
        where TDatabaseModel : DatabaseModel
    {
        public TDbContext Context { get; }

        public DbSet<TDatabaseModel> DbSet => Context.Set<TDatabaseModel>()!;

        protected Repository([NotNull] TDbContext context) => Context = context;

        public async Task<bool> ContainsByKeyAsync(IEnumerable keys) => await ContainsByKeyAsync(keys, CancellationToken.None);

        public async Task<bool> ContainsByKeyAsync(IEnumerable keys, CancellationToken token) =>
            !(await FindByKeys(keys, token) is null);

        public async ValueTask<TDatabaseModel> FindByKeys(IEnumerable keys) =>
            await FindByKeys(keys, CancellationToken.None);

        public async ValueTask<TDatabaseModel> FindByKeys(IEnumerable keys, CancellationToken token) =>
            (await DbSet.FindAsync(keys, token))!;

        public void Remove(params TDatabaseModel[] models) => Remove((IEnumerable<TDatabaseModel>)models);

        public void Remove(IEnumerable<TDatabaseModel> models) => DbSet.RemoveRange(models);

        public async Task RemoveAllAsync() => await Context.DeleteTableFromDbSetAsync<TDatabaseModel>();

        public void Update(params TDatabaseModel[] models) => Update((IEnumerable<TDatabaseModel>)models);

        public void Update(IEnumerable<TDatabaseModel> models) => DbSet.UpdateRange(models);

        public async Task<int> DbSync() => await DbSync(CancellationToken.None);

        public async Task<int> DbSync(CancellationToken token) =>
            await Context.SaveChangesAsync(token)!;
    }
}