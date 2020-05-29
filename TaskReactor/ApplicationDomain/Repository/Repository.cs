using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Models.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.Repository
{
    public abstract class Repository<TDataBaseModel, TDbContext> where TDbContext : DbContext
        where TDataBaseModel : DatabaseModel
    {
        [NotNull] public readonly TDbContext Context;

        [NotNull] public DbSet<TDataBaseModel> DbSet => Context.Set<TDataBaseModel>()!;

        protected Repository([NotNull] TDbContext context) => Context = context;

        public async Task<bool> ContainsByKey(IEnumerable keys) => await ContainsByKey(keys, CancellationToken.None);

        public async Task<bool> ContainsByKey(IEnumerable keys, CancellationToken token) =>
            !(await FindByKeys(keys, token) is null);

        public async ValueTask<TDataBaseModel> FindByKeys(IEnumerable keys) =>
            await FindByKeys(keys, CancellationToken.None);

        public async ValueTask<TDataBaseModel> FindByKeys(IEnumerable keys, CancellationToken token) =>
            (await DbSet.FindAsync(keys, token))!;

        public void Remove([NotNull] params TDataBaseModel[] models) => Remove((IEnumerable<TDataBaseModel>)models);

        public void Remove([NotNull] IEnumerable<TDataBaseModel> models) => DbSet.RemoveRange(models);

        public void Update([NotNull] params TDataBaseModel[] models) => Update((IEnumerable<TDataBaseModel>)models);

        public void Update([NotNull] IEnumerable<TDataBaseModel> models) => DbSet.UpdateRange(models);

        [NotNull]
        public async Task<int> DbSync() => await DbSync(CancellationToken.None);

        [NotNull]
        public async Task<int> DbSync(CancellationToken cancellationToken) =>
            await Context.SaveChangesAsync(cancellationToken)!;
    }
}