using System;
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
    sealed class GoalService : Service<Goal, TaskReactorDbContext, IGoalRepository, GoalModel>, IGoalService
    {
        private class GoalModelComparer : Comparer<GoalModel>
        {
            public override int Compare(GoalModel x, GoalModel y) =>
                x?.Identity.CompareTo(y?.Identity ?? 0) ?? 0;
        }

        [NotNull] readonly GoalModelComparer _comparer = new GoalModelComparer();

        [NotNull] readonly IGoalModelNotificationService _service;

        private Action<GoalModel> _notifyAction;

        public Action<GoalModel> NotifyAction
        {
            get => _notifyAction;
            set
            {
                if (!(value is null))
                    foreach (var model in _service.Models)
                        _service.UpdateModelAction(model, value);

                _notifyAction = value;
            }
        }

        [ImportingConstructor]
        public GoalService(
            [NotNull] IGoalRepository repository,
            [NotNull] IGoalModelNotificationService service
        ) : base(repository) => _service = service;

        public async Task<List<GoalModel>> GetAllFromTaskAsync(UserTaskModel task) =>
            await GetAllFromTaskAsync(task, CancellationToken.None);

        public async Task<List<GoalModel>> GetAllFromTaskAsync(UserTaskModel task, CancellationToken token)
        {
            var goalList = (from goal in await Repository.GetAllFromTaskAsync(task._dataBaseModel, token)
                select CreateModelInstance(goal)).ToList();

            var toBeRemoved = _service.Models.ToList();
            // ReSharper disable PossibleNullReferenceException
            toBeRemoved.Sort((left, right) => left._dataBaseModel.Id.CompareTo(right.Identity));
            // ReSharper restore PossibleNullReferenceException

            foreach (var model in goalList)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                _service.UpdateModelAction(model, NotifyAction ?? (_ => {}));
                var index = toBeRemoved.BinarySearch(model, _comparer);
                if (index >= 0) toBeRemoved.RemoveAt(index);
            }

            foreach (var model in toBeRemoved)
                // ReSharper disable once AssignNullToNotNullAttribute
                _service.RemoveModel(model);

            return goalList;
        }

        public void AddToTask(UserTaskModel userTask, params GoalModel[] goals) =>
            AddToTask(userTask, (IEnumerable<GoalModel>)goals);

        public void AddToTask(UserTaskModel userTask, IEnumerable<GoalModel> goals)
        {
            foreach (var goal in goals)
            {
                _service.UpdateModelAction(goal, NotifyAction ?? (_ => {}));
                Repository.AddToTask(userTask._dataBaseModel, goal._dataBaseModel);
            }
        }

        public override void Update(IEnumerable<GoalModel> models)
        {
            foreach (var model in models)
            {
                _service.UpdateModelAction(model, NotifyAction ?? (_ => {}));
                Repository.Update(model._dataBaseModel);
            }
        }
    }
}