using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Data.Database;
using Data.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Data.DataRepository
{
    abstract class Repository<TDatabaseModel, TDbContext> : IRepository<TDatabaseModel, TDbContext>
        where TDbContext : DbContext
        where TDatabaseModel : DatabaseModel
    {
        public TDbContext Context { get; }

        public DbSet<TDatabaseModel> DbSet => Context.Set<TDatabaseModel>()!;

        protected Repository([NotNull] TDbContext context) => Context = context;

        public async Task<bool> ContainsByKeyAsync(IEnumerable keys) =>
            await ContainsByKeyAsync(keys, CancellationToken.None);

        public async Task<bool> ContainsByKeyAsync(IEnumerable keys, CancellationToken token) =>
            !(await FindByKeysAsync(keys, token) is null);

        public async Task<TDatabaseModel> FindByKeysAsync(params object[] keys) =>
            await FindByKeysAsync(keys, CancellationToken.None);

        public async Task<TDatabaseModel> FindByKeysAsync(IEnumerable keys) =>
            await FindByKeysAsync(keys, CancellationToken.None);

        public Task<TDatabaseModel> FindByKeysAsync(IEnumerable keys, CancellationToken token) =>
            Task.Run(async () => await DbSet.FindAsync(keys)!, token);

        public Task<TDatabaseModel> FindByKeysAsync(CancellationToken token, params object[] keys) =>
            Task.Run(async () => await DbSet.FindAsync(keys)!, token);

        public void Remove(params TDatabaseModel[] models) => Remove((IEnumerable<TDatabaseModel>)models);

        public void Remove(IEnumerable<TDatabaseModel> models) => DbSet.RemoveRange(models);

        public async Task RemoveAllAsync() => await RemoveAllAsync(CancellationToken.None);

        public Task RemoveAllAsync(CancellationToken token) =>
            Task.Run(() => Context.DeleteTableFromDbSet<TDatabaseModel>(), token);

        public void Update(params TDatabaseModel[] models) => Update((IEnumerable<TDatabaseModel>)models);

        public void Update(IEnumerable<TDatabaseModel> models) => DbSet.UpdateRange(models);

        public async Task<int> DbSync() => await DbSync(CancellationToken.None);

        public async Task<int> DbSync(CancellationToken token) =>
            await Context.SaveChangesAsync(token)!;
    }
}