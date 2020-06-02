using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataModel;
using ApplicationDomain.DataRepository;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.ModelService
{
    public abstract class Service<TDatabaseModel, TDbContext, TRepository, TModel> : 
        IService<TDatabaseModel, TDbContext, TRepository, TModel> 
        where TDatabaseModel : DatabaseModel, new()
        where TDbContext : DbContext
        where TRepository : IRepository<TDatabaseModel, TDbContext>
        where TModel : Model<TDatabaseModel>, new()
    {
        public TRepository Repository { get; }

        protected Service([NotNull] TRepository repository) => Repository = repository;

        public async Task<bool> ContainsByKeyAsync(IEnumerable keys) =>
            await ContainsByKeyAsync(keys, CancellationToken.None);

        public async Task<bool> ContainsByKeyAsync(IEnumerable keys, CancellationToken token) =>
            await Repository.ContainsByKeyAsync(keys, token);

        public async ValueTask<TModel> FindByKeysAsync(IEnumerable keys) =>
            await FindByKeysAsync(keys, CancellationToken.None);

        public async ValueTask<TModel> FindByKeysAsync(IEnumerable keys, CancellationToken token) =>
            ModelConstructionProvider<TModel, TDatabaseModel>.CreateModelInstance(
                await Repository.FindByKeysAsync(keys, token)
            );

        public void Remove(params TModel[] models) => Remove((IEnumerable<TModel>)models);

        public void Remove(IEnumerable<TModel> models) =>
            Repository.Remove(from model in models select model._dataBaseModel);

        public async Task RemoveAllAsync() => await Repository.Context.DeleteTableFromDbSetAsync<TDatabaseModel>();

        public void Update(params TModel[] models) => Update((IEnumerable<TModel>)models);

        public void Update(IEnumerable<TModel> models) =>
            Repository.Update(from model in models select model._dataBaseModel);

        public async Task<int> DbSync() => await DbSync(CancellationToken.None);

        public async Task<int> DbSync(CancellationToken token) => await Repository.DbSync(token)!;
    }
}