using System.Collections;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Utilities
{
    public abstract class Repository<TDataBaseModel, TDbContext> where TDataBaseModel : DataBaseModel where TDbContext : DbContext
    {
        [NotNull] public readonly TDbContext Context;

        [NotNull] public DbSet<TDataBaseModel> DbSet => Context.Set<TDataBaseModel>()!;

        protected Repository([NotNull] TDbContext context) => Context = context;

        public async void FindByKeys(IEnumerable keys)
        {
            await DbSet.FindAsync(keys);
        }
    }
}