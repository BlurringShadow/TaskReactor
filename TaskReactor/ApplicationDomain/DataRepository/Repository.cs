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

        public async Task<TDatabaseModel> FindByKeysAsync(IEnumerable keys, CancellationToken token) =>
            await Task.Run(
                () =>
                {
                    lock(Context) return DbSet.Find(keys)!;
                }, token
            );

        public async Task<TDatabaseModel> FindByKeysAsync(CancellationToken token, params object[] keys) =>
            await Task.Run(
                () =>
                {
                    lock(Context) return DbSet.Find(keys)!;
                }, token
            );

        public void Remove(params TDatabaseModel[] models) => Remove((IEnumerable<TDatabaseModel>)models);

        public void Remove(IEnumerable<TDatabaseModel> models)
        {
            lock(Context) DbSet.RemoveRange(models);
        }

        public async Task RemoveAllAsync() => await RemoveAllAsync(CancellationToken.None);

        public async Task RemoveAllAsync(CancellationToken token) =>
            await Task.Run(
                () =>
                {
                    lock(Context) Context.DeleteTableFromDbSet<TDatabaseModel>();
                }, token
            );

        public void Update(params TDatabaseModel[] models) => Update((IEnumerable<TDatabaseModel>)models);

        public void Update(IEnumerable<TDatabaseModel> models)
        {
            lock(Context) DbSet.UpdateRange(models);
        }

        public async Task<int> DbSync() => await DbSync(CancellationToken.None);

        public async Task<int> DbSync(CancellationToken token) =>
            await Task.Run(
                () =>
                {
                    lock(Context) return Context.SaveChanges()!;
                }, token
            );
    }
}