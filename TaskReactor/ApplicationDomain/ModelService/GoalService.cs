using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataModel;
using ApplicationDomain.DataRepository;
using JetBrains.Annotations;

namespace ApplicationDomain.ModelService
{
    sealed class GoalService : Service<Goal, TaskReactorDbContext, IGoalRepository, GoalModel>, IGoalService
    {
        [ImportingConstructor]
        public GoalService([NotNull] IGoalRepository repository) : base(repository)
        {
        }

        public async Task<List<GoalModel>> GetAllFromTaskAsync(UserTaskModel task) =>
            await GetAllFromTaskAsync(task, CancellationToken.None);

        public async Task<List<GoalModel>> GetAllFromTaskAsync(UserTaskModel task, CancellationToken token) =>
            (from goal in await Repository.GetAllFromTaskAsync(task._dataBaseModel, token)
                select CreateModelInstance(goal)).ToList();

        public void AddToTask(UserTaskModel userTask, params GoalModel[] goals) =>
            AddToTask(userTask, (IEnumerable<GoalModel>)goals);

        public void AddToTask(UserTaskModel userTask, IEnumerable<GoalModel> goals) =>
            Repository.AddToTask(userTask._dataBaseModel, from goal in goals select goal._dataBaseModel);
    }
}