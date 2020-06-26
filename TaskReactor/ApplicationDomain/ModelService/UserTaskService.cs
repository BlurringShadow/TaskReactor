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
    sealed class UserTaskService :
        Service<UserTask, TaskReactorDbContext, IUserTaskRepository, UserTaskModel>,
        IUserTaskService
    {
        private class UserTaskModelComparer : Comparer<UserTaskModel>
        {
            public override int Compare(UserTaskModel x, UserTaskModel y) =>
                x?.Identity.CompareTo(y?.Identity ?? 0) ?? 0;
        }

        [NotNull] readonly UserTaskModelComparer _comparer = new UserTaskModelComparer();

        [NotNull] readonly IUserTaskModelNotificationService _service;

        private Action<UserTaskModel> _notifyAction;

        public Action<UserTaskModel> NotifyAction
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
        public UserTaskService(
            [NotNull] IUserTaskRepository repository,
            [NotNull] IUserTaskModelNotificationService service
        ) : base(repository) => _service = service;

        public async Task<IList<UserTaskModel>> GetAllFromUserAsync(UserModel user) =>
            await GetAllFromUserAsync(user, CancellationToken.None);

        public async Task<IList<UserTaskModel>> GetAllFromUserAsync(UserModel user, CancellationToken token)
        {
            var taskList = (from userTask in await Repository.GetAllFromUserAsync(user._dataBaseModel, token)
                select CreateModelInstance(userTask)).ToList();

            var toBeRemoved = _service.Models.ToList();
            // ReSharper disable PossibleNullReferenceException
            toBeRemoved.Sort((left, right) => left.Identity.CompareTo(right.Identity));
            // ReSharper restore PossibleNullReferenceException

            foreach (var model in taskList)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                _service.UpdateModelAction(model, NotifyAction ?? (_ => {}));

                var index = toBeRemoved.BinarySearch(model, _comparer);
                if (index >= 0) toBeRemoved.RemoveAt(index);
            }

            foreach (var model in toBeRemoved)
                // ReSharper disable once AssignNullToNotNullAttribute
                _service.RemoveModel(model);

            return taskList;
        }

        public void AddToUser(UserModel user,
            params UserTaskModel[] userTasks) =>
            AddToUser(user, (IEnumerable<UserTaskModel>)userTasks);

        public void AddToUser(UserModel user, IEnumerable<UserTaskModel> userTasks)
        {
            foreach (var userTask in userTasks)
            {
                _service.UpdateModelAction(userTask, NotifyAction ?? (_ => {}));
                Repository.AddToUser(user._dataBaseModel, userTask._dataBaseModel);
            }
        }

        public override void Update(IEnumerable<UserTaskModel> models)
        {
            foreach (var model in models)
            {
                _service.UpdateModel(model);
                Repository.Update(model._dataBaseModel);
            }
        }
    }
}