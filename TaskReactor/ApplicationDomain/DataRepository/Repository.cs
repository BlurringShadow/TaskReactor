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
    public abstract class Repository<TDataBaseModel, TDbContext> : IRepository<TDataBaseModel, TDbContext>
        where TDbContext : DbContext
        where TDataBaseModel : DatabaseModel
    {
        public TDbContext Context { get; }

        public DbSet<TDataBaseModel> DbSet => Context.Set<TDataBaseModel>()!;

        protected Repository([NotNull] TDbContext context) => Context = context;

        public async Task<bool> ContainsByKeyAsync(IEnumerable keys) => await ContainsByKeyAsync(keys, CancellationToken.None);

        public async Task<bool> ContainsByKeyAsync(IEnumerable keys, CancellationToken token) =>
            !(await FindByKeys(keys, token) is null);

        public async ValueTask<TDataBaseModel> FindByKeys(IEnumerable keys) =>
            await FindByKeys(keys, CancellationToken.None);

        public async ValueTask<TDataBaseModel> FindByKeys(IEnumerable keys, CancellationToken token) =>
            (await DbSet.FindAsync(keys, token))!;

        public void Remove(params TDataBaseModel[] models) => Remove((IEnumerable<TDataBaseModel>)models);

        public void Remove(IEnumerable<TDataBaseModel> models) => DbSet.RemoveRange(models);

        public async Task RemoveAllAsync() => await Context.DeleteTableFromDbSetAsync<TDataBaseModel>();

        public void Update(params TDataBaseModel[] models) => Update((IEnumerable<TDataBaseModel>)models);

        public void Update(IEnumerable<TDataBaseModel> models) => DbSet.UpdateRange(models);

        public async Task<int> DbSync() => await DbSync(CancellationToken.None);

        public async Task<int> DbSync(CancellationToken token) =>
            await Context.SaveChangesAsync(token)!;
    }
}