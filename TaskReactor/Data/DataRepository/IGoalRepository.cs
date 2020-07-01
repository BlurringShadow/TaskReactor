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
    public interface IGoalRepository : IRepository<Goal, TaskReactorDbContext>
    {
        /// <summary>
        /// Get all goals from task
        /// </summary>
        /// <param name="task"> Input task </param>
        /// <returns> Async result of list goals </returns>
        [NotNull, ItemNotNull]
        Task<IList<Goal>> GetAllFromTaskAsync([NotNull] UserTask task);

        /// <summary>
        /// Get all goals from task
        /// </summary>
        /// <param name="task"> Input task </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Async result of list goals </returns>
        [NotNull, ItemNotNull]
        Task<IList<Goal>> GetAllFromTaskAsync([NotNull] UserTask task, CancellationToken token);


        /// <summary>
        /// Add goals to task
        /// </summary>
        void AddToTask([NotNull] UserTask userTask, [NotNull, ItemNotNull] params Goal[] goals);

        /// <summary>
        /// Add goals to task
        /// </summary>
        void AddToTask([NotNull] UserTask userTask, [NotNull, ItemNotNull] IEnumerable<Goal> goals);
    }
}