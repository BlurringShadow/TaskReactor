using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using Data.Database;
using Data.Database.Entity;
using Data.DataRepository;
using JetBrains.Annotations;

namespace ApplicationDomain.ModelService
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    sealed class TaskDependencyService : Service<TaskDependency, TaskReactorDbContext, ITaskDependencyRepository,
        TaskDependencyModel>, ITaskDependencyService
    {
        [ImportingConstructor]
        public TaskDependencyService([NotNull] ITaskDependencyRepository repository) : base(repository)
        {
        }

        public async Task<List<TaskDependencyModel>> GetDependenciesAsync(UserTaskModel task) =>
            await GetDependenciesAsync(task, CancellationToken.None);

        public async Task<List<TaskDependencyModel>> GetDependenciesAsync(
            UserTaskModel task,
            CancellationToken token
        ) => (from taskDependency in await Repository.GetDependenciesAsync(task._dataBaseModel, token)
            select CreateModelInstance(taskDependency)).ToList();

        public IList<TaskDependencyModel> AddDependencies(
            UserTaskModel target,
            params UserTaskModel[] userTasks
        ) => AddDependencies(target, (IEnumerable<UserTaskModel>)userTasks);

        public IList<TaskDependencyModel> AddDependencies(
            UserTaskModel target,
            IEnumerable<UserTaskModel> userTasks
        ) => (from taskDependency in Repository.AddDependencies(
                target._dataBaseModel, from userTaskModel in userTasks select userTaskModel._dataBaseModel
            )
            select CreateModelInstance(taskDependency)).ToList();
    }
}