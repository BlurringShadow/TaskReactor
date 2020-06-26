using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Data.Database;
using Data.Database.Entity;
using JetBrains.Annotations;

namespace Data.DataRepository
{
    [InheritedExport]
    public interface IUserTaskRepository : IRepository<UserTask, TaskReactorDbContext>
    {
        /// <summary>
        /// Get all tasks from user
        /// </summary>
        /// <param name="user"> The user </param>
        /// <returns> Async result contains list of <see cref="Data.Database.Entity.UserTask"/> </returns>
        [NotNull, ItemNotNull]
        Task<IList<UserTask>> GetAllFromUserAsync([NotNull] User user);

        /// <summary>
        /// Get all tasks from user
        /// </summary>
        /// <param name="user"> The user </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Async result contains list of <see cref="Data.Database.Entity.UserTask"/> </returns>
        [NotNull, ItemNotNull]
        Task<IList<UserTask>> GetAllFromUserAsync([NotNull] User user, CancellationToken token);

        /// <summary>
        /// Add tasks to user
        /// </summary>
        void AddToUser([NotNull] User user, [NotNull, ItemNotNull] params UserTask[] userTasks);

        /// <summary>
        /// Add tasks to user
        /// </summary>
        void AddToUser([NotNull] User user, [NotNull, ItemNotNull] IEnumerable<UserTask> userTasks);
    }
}