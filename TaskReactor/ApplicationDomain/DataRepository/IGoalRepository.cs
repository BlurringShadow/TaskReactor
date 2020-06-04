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
    public interface IGoalRepository : IRepository<Goal, TaskReactorDbContext>
    {
        /// <summary>
        /// Get all goals from task
        /// </summary>
        /// <param name="task"> Input task </param>
        /// <returns> Async result of list goals </returns>
        [NotNull, ItemNotNull]
        Task<List<Goal>> GetAllFromTaskAsync([NotNull] UserTask task);

        /// <summary>
        /// Get all goals from task
        /// </summary>
        /// <param name="task"> Input task </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Async result of list goals </returns>
        [NotNull, ItemNotNull]
        Task<List<Goal>> GetAllFromTaskAsync([NotNull] UserTask task, CancellationToken token);


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