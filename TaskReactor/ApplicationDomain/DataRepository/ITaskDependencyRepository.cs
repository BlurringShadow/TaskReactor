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
    public interface ITaskDependencyRepository : IRepository<TaskDependency, TaskReactorDbContext>
    {
        /// <summary>
        /// Get task dependencies
        /// </summary>
        [NotNull, ItemNotNull]
        Task<IList<TaskDependency>> GetDependenciesAsync([NotNull] UserTask task);

        /// <summary>
        /// Get task dependencies
        /// </summary>
        [NotNull, ItemNotNull]
        Task<IList<TaskDependency>> GetDependenciesAsync([NotNull] UserTask task, CancellationToken token);

        /// <summary>
        /// Add task dependencies to task
        /// </summary>
        [NotNull, ItemNotNull]
        IList<TaskDependency> AddDependencies([NotNull] UserTask target,
            [NotNull, ItemNotNull] params UserTask[] userTasks);

        /// <summary>
        /// Add task dependencies to task
        /// </summary>
        [NotNull, ItemNotNull]
        IList<TaskDependency> AddDependencies(
            [NotNull] UserTask target,
            [NotNull, ItemNotNull] IEnumerable<UserTask> userTasks
        );
    }
}