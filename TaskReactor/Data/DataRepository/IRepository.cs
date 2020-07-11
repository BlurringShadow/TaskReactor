using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Data.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Data.DataRepository
{
    /// <summary>
    /// Provide basic data operation
    /// <para/> Modifier database data actions are not immediately complete
    /// </summary>
    /// <typeparam name="TDataBaseModel"> database entity model </typeparam>
    /// <typeparam name="TDbContext"> database context </typeparam>
    public interface IRepository<TDataBaseModel, out TDbContext>
        where TDataBaseModel : DatabaseModel
        where TDbContext : DbContext

    {
        /// <summary>
        /// Database context <see cref="DbContext"/>
        /// <para/> Be aware of using it in multi-threading code context.
        /// </summary>
        [NotNull] TDbContext Context { get; }

        /// <summary>
        /// Entity set <see cref="DbSet{T}"/>
        /// </summary>
        [NotNull] DbSet<TDataBaseModel> DbSet { get; }

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
        [NotNull]
        Task<TDataBaseModel> FindByKeysAsync(IEnumerable keys);

        /// <summary>
        /// Find with keys
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Return an async task with finding result. </returns>
        [NotNull]
        Task<TDataBaseModel> FindByKeysAsync(IEnumerable keys, CancellationToken token);

        /// <summary>
        /// Find with keys
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <returns> Return an async task with finding result. </returns>
        [NotNull]
        Task<TDataBaseModel> FindByKeysAsync([NotNull, ItemNotNull] params object[] keys);

        /// <summary>
        /// Find with keys
        /// </summary>
        /// <param name="keys"> input keys </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Return an async task with finding result. </returns>
        [NotNull]
        Task<TDataBaseModel> FindByKeysAsync(CancellationToken token, [NotNull, ItemNotNull] params object[] keys);

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
        /// Remove all the entities
        /// </summary>
        /// <param name="token"></param>
        /// <returns> Async running task </returns>
        [NotNull]
        Task RemoveAllAsync(CancellationToken token);

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
        /// <para> Be aware of using it in multi-threading code context. </para>
        /// </summary>
        /// <returns> Async task with affected rows </returns>
        [NotNull]
        Task<int> DbSync();

        /// <summary>
        /// Sync changes into database.
        /// <para> Be aware of using it in multi-threading code context. </para>
        /// </summary>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Async task with affected rows </returns>
        [NotNull]
        Task<int> DbSync(CancellationToken token);
    }
}