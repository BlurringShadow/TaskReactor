using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.DataRepository
{
    /// <summary>
    /// Provide basic data operation
    /// </summary>
    /// <typeparam name="TDataBaseModel"> database entity model </typeparam>
    /// <typeparam name="TDbContext"> database context </typeparam>
    public interface IRepository<TDataBaseModel, out TDbContext>
        where TDataBaseModel : DatabaseModel 
        where TDbContext : DbContext

    {
        /// <summary>
        /// Database context <see cref="DbContext"/>
        /// </summary>
        [NotNull] DbSet<TDataBaseModel> DbSet { get; }

        /// <summary>
        /// Entity set <see cref="DbSet{T}"/>
        /// </summary>
        [NotNull] TDbContext Context { get; }

        /// <summary>
        /// Find if table contains the key.
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <returns> Return an async task and task.value is true if contains. </returns>
        [NotNull]
        Task<bool> ContainsByKeyAsync(IEnumerable keys);

        /// <summary>
        /// Find if table contains the key
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Return an async task and task.value is true if contains. </returns>
        [NotNull]
        Task<bool> ContainsByKeyAsync(IEnumerable keys, CancellationToken token);

        /// <summary>
        /// Find with keys
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <returns> Return an async task with finding result. </returns>
        ValueTask<TDataBaseModel> FindByKeysAsync(IEnumerable keys);

        /// <summary>
        /// Find with keys
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Return an async task with finding result. </returns>
        ValueTask<TDataBaseModel> FindByKeysAsync(IEnumerable keys, CancellationToken token);

        /// <summary>
        /// Remove the entities
        /// </summary>
        void Remove([NotNull] IEnumerable<TDataBaseModel> models);

        /// <summary>
        /// Remove the entities
        /// </summary>
        void Remove([NotNull] params TDataBaseModel[] models);

        /// <summary>
        /// Remove all the entities
        /// </summary>
        /// <returns> Async running task </returns>
        [NotNull]
        Task RemoveAllAsync();

        /// <summary>
        /// Update or add the entities
        /// </summary>
        void Update([NotNull, ItemNotNull] params TDataBaseModel[] models);

        /// <summary>
        /// Update or add the entities
        /// </summary>
        void Update([NotNull, ItemNotNull] IEnumerable<TDataBaseModel> models);


        /// <summary>
        /// Sync changes into database
        /// </summary>
        /// <returns> Async task with affected rows </returns>
        [NotNull]
        Task<int> DbSync();


        /// <summary>
        /// Sync changes into database
        /// </summary>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Async task with affected rows </returns>
        [NotNull]
        Task<int> DbSync(CancellationToken token);
    }
}