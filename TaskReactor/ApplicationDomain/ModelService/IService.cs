using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataModel;
using ApplicationDomain.DataRepository;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationDomain.ModelService
{
    public interface IService<TDatabaseModel, out TDbContext, out TRepository, TModel>
        where TDatabaseModel : DatabaseModel, new()
        where TDbContext : DbContext
        where TRepository : IRepository<TDatabaseModel, TDbContext>
        where TModel : Model<TDatabaseModel>, new()
    {
        /// <summary>
        /// <see cref="IRepository{T, U}"/>
        /// </summary>
        [NotNull] TRepository Repository { get; }

        /// <summary>
        /// Find if table contains the key. Input <paramref name="keys"/> should match the entity key type order
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <returns> Return an async task and task.value is true if contains. </returns>
        [NotNull]
        Task<bool> ContainsByKeyAsync([NotNull] IEnumerable keys);

        /// <summary>
        /// Find if table contains the key. Input <paramref name="keys"/> should match the entity key type order
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Return an async task and task.value is true if contains. </returns>
        [NotNull]
        Task<bool> ContainsByKeyAsync([NotNull] IEnumerable keys, CancellationToken token);


        /// <summary>
        /// Find if table contains the key. Input <paramref name="keys"/> should match the entity key type order
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <returns> Return an async task with finding result. </returns>
        ValueTask<TModel> FindByKeysAsync([NotNull] IEnumerable keys);

        /// <summary>
        /// Find if table contains the key. Input <paramref name="keys"/> should match the entity key type order
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Return an async task with finding result. </returns>
        ValueTask<TModel> FindByKeysAsync([NotNull] IEnumerable keys, CancellationToken token);

        /// <summary>
        /// Remove the models in database
        /// </summary>
        void Remove([ItemNotNull, NotNull] IEnumerable<TModel> models);

        /// <summary>
        /// Remove the models in database
        /// </summary>
        void Remove([ItemNotNull, NotNull] params TModel[] models);

        /// <summary>
        /// Remove all the models in database
        /// </summary>
        /// <returns> Async running task </returns>
        [NotNull]
        Task RemoveAllAsync();

        /// <summary>
        /// Update the models in database
        /// </summary>
        void Update([ItemNotNull, NotNull] IEnumerable<TModel> models);

        /// <summary>
        /// Update the models in database
        /// </summary>
        void Update([ItemNotNull, NotNull] params TModel[] models);

        /// <summary>
        /// Sync changes into database
        /// </summary>
        /// <returns> Async task with affected rows </returns>
        [NotNull]
        Task<int> DbSync();

        /// <summary>
        /// Sync changes into database
        /// </summary>
        /// <returns> Async task with affected rows </returns>
        [NotNull]
        Task<int> DbSync(CancellationToken token);
    }
}