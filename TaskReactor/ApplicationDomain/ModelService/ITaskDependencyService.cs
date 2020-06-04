using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using JetBrains.Annotations;

namespace ApplicationDomain.ModelService
{
    [InheritedExport]
    public interface ITaskDependencyService
    {
        /// <summary>
        /// Get task dependencies
        /// </summary>
        [NotNull, ItemNotNull]
        Task<List<TaskDependencyModel>> GetDependenciesAsync([NotNull] UserTaskModel task);

        /// <summary>
        /// Get task dependencies
        /// </summary>
        [NotNull, ItemNotNull]
        Task<List<TaskDependencyModel>> GetDependenciesAsync(
            [NotNull] UserTaskModel task,
            CancellationToken token
        );

        /// <summary>
        /// Add task dependencies to task
        /// </summary>
        [NotNull, ItemNotNull]
        IList<TaskDependencyModel> AddDependencies(
            [NotNull] UserTaskModel target,
            [NotNull, ItemNotNull] params UserTaskModel[] userTasks
        );

        /// <summary>
        /// Add task dependencies to task
        /// </summary>
        [NotNull, ItemNotNull]
        IList<TaskDependencyModel> AddDependencies(
            [NotNull] UserTaskModel target,
            [NotNull, ItemNotNull] IEnumerable<UserTaskModel> userTasks
        );
    }
}