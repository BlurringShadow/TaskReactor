using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using Data.Database;
using Data.Database.Entity;
using Data.DataRepository;
using JetBrains.Annotations;

namespace ApplicationDomain.ModelService
{
    [InheritedExport]
    public interface ITaskDependencyService : 
        IService<TaskDependency, TaskReactorDbContext, ITaskDependencyRepository, TaskDependencyModel>
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