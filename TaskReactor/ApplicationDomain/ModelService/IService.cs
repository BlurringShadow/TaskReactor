using ApplicationDomain.DataModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Data.Database.Entity;
using Data.DataRepository;

namespace ApplicationDomain.ModelService
{
    public interface IService<TDatabaseModel, out TDbContext, out TRepository, TModel>
        where TDatabaseModel : DatabaseModel, new()
        where TDbContext : DbContext
        where TRepository : IRepository<TDatabaseModel, TDbContext>
        where TModel : Model<TDatabaseModel>, new()
    {
        /// <summary>
        /// <see cref="Data.DataRepository.IRepository{TDataBaseModel,TDbContext}"/>
        /// </summary>
        [NotNull] public TRepository Repository { get; }

        /// <summary>
        /// Find if table contains the key. Input <paramref name="keys"/> should match the entity key type order
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <returns> Return an async task and task.value is true if contains. </returns>
        [NotNull]
        public Task<bool> ContainsByKeyAsync([NotNull] IEnumerable keys);

        /// <summary>
        /// Find if table contains the key. Input <paramref name="keys"/> should match the entity key type order
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Return an async task and task.value is true if contains. </returns>
        [NotNull]
        public Task<bool> ContainsByKeyAsync([NotNull] IEnumerable keys, CancellationToken token);


        /// <summary>
        /// Find if table contains the key. Input <paramref name="keys"/> should match the entity key type order
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <returns> Return an async task with finding result. </returns>
        public ValueTask<TModel> FindByKeysAsync([NotNull] IEnumerable keys);

        /// <summary>
        /// Find if table contains the key. Input <paramref name="keys"/> should match the entity key type order
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Return an async task with finding result. </returns>
        public ValueTask<TModel> FindByKeysAsync([NotNull] IEnumerable keys, CancellationToken token);

        /// <summary>
        /// Remove the models in database
        /// </summary>
        public void Remove([ItemNotNull, NotNull] IEnumerable<TModel> models);

        /// <summary>
        /// Remove the models in database
        /// </summary>
        public void Remove([ItemNotNull, NotNull] params TModel[] models);

        /// <summary>
        /// Remove all the models in database
        /// </summary>
        /// <returns> Async running task </returns>
        [NotNull]
        public Task RemoveAllAsync();

        /// <summary>
        /// Update the models in database
        /// </summary>
        public void Update([ItemNotNull, NotNull] IEnumerable<TModel> models);

        /// <summary>
        /// Update the models in database
        /// </summary>
        public void Update([ItemNotNull, NotNull] params TModel[] models);

        /// <summary>
        /// Sync changes into database
        /// </summary>
        /// <returns> Async task with affected rows </returns>
        [NotNull]
        public Task<int> DbSync();

        /// <summary>
        /// Sync changes into database
        /// </summary>
        /// <returns> Async task with affected rows </returns>
        [NotNull]
        public Task<int> DbSync(CancellationToken token);
    }
}