using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using Data.Database;
using Data.Database.Entity;
using Data.DataRepository;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.ModelService
{
    abstract class Service<TDatabaseModel, TDbContext, TRepository, TModel> :
        IService<TDatabaseModel, TDbContext, TRepository, TModel>
        where TDatabaseModel : DatabaseModel, new()
        where TDbContext : DbContext
        where TRepository : IRepository<TDatabaseModel, TDbContext>
        where TModel : Model<TDatabaseModel>, new()
    {
        [NotNull]
        protected static TModel CreateModelInstance([NotNull] TDatabaseModel databaseModel) =>
            ModelConstructionProvider<TModel, TDatabaseModel>.CreateModelInstance(databaseModel);

        public TRepository Repository { get; }

        protected Service([NotNull] TRepository repository) => Repository = repository;

        public async Task<bool> ContainsByKeyAsync(IEnumerable keys) =>
            await ContainsByKeyAsync(keys, CancellationToken.None);

        public async Task<bool> ContainsByKeyAsync(IEnumerable keys, CancellationToken token) =>
            await Repository.ContainsByKeyAsync(keys, token);

        public async ValueTask<TModel> FindByKeysAsync(IEnumerable keys) =>
            await FindByKeysAsync(keys, CancellationToken.None);

        public async ValueTask<TModel> FindByKeysAsync(IEnumerable keys, CancellationToken token)
        {
            var result = await Repository.FindByKeysAsync(keys, token);
            return result is null ? null : CreateModelInstance(result);
        }

        public async ValueTask<TModel> FindByKeysAsync(params object[] keys) =>
            await FindByKeysAsync(CancellationToken.None, keys);

        public async ValueTask<TModel> FindByKeysAsync(CancellationToken token, params object[] keys)
        {
            var result = await Repository.FindByKeysAsync(token, keys);
            return result is null ? null : CreateModelInstance(result);
        }

        public void Remove(params TModel[] models) => Remove((IEnumerable<TModel>)models);

        public virtual void Remove(IEnumerable<TModel> models) =>
            Repository.Remove(from model in models select model._dataBaseModel);

        public async Task RemoveAllAsync() => await Repository.Context.DeleteTableFromDbSetAsync<TDatabaseModel>();

        public void Update(params TModel[] models) => Update((IEnumerable<TModel>)models);

        public virtual void Update(IEnumerable<TModel> models) =>
            Repository.Update(from model in models select model._dataBaseModel);

        public async Task<int> DbSync() => await DbSync(CancellationToken.None);

        public virtual async Task<int> DbSync(CancellationToken token) => await Repository.DbSync(token)!;
    }
}