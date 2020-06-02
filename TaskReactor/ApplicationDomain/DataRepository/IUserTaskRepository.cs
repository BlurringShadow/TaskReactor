using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataRepository
{
    [InheritedExport]
    public interface IUserTaskRepository : IRepository<UserTask, TaskReactorDbContext>
    {
        /// <summary>
        /// Get all tasks from user
        /// </summary>
        /// <param name="user"> The user </param>
        /// <returns> Async result contains list of <see cref="UserTask"/> </returns>
        [NotNull, ItemNotNull]
        Task<List<UserTask>> GetAllFromUserAsync([NotNull] User user);

        /// <summary>
        /// Get all tasks from user
        /// </summary>
        /// <param name="user"> The user </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Async result contains list of <see cref="UserTask"/> </returns>
        [NotNull, ItemNotNull]
        Task<List<UserTask>> GetAllFromUserAsync([NotNull] User user, CancellationToken token);

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