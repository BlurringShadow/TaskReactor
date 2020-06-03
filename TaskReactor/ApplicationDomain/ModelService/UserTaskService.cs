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
    sealed class UserTaskService : Service<UserTask, TaskReactorDbContext, IUserTaskRepository, UserTaskModel>,
        IUserTaskService
    {
        [ImportingConstructor]
        public UserTaskService([NotNull] IUserTaskRepository repository) : base(repository)
        {
        }

        public async Task<IList<UserTask>> GetAllFromUserAsync(UserModel user) =>
            await GetAllFromUserAsync(user, CancellationToken.None);

        public async Task<IList<UserTask>> GetAllFromUserAsync(UserModel user, CancellationToken token) =>
            await Repository.GetAllFromUserAsync(user._dataBaseModel, token);
 
        public void AddToUser(UserModel user, params UserTaskModel[] userTasks) => 
            AddToUser(user, (IEnumerable<UserTaskModel>)userTasks);

        public void AddToUser(UserModel user, IEnumerable<UserTaskModel> userTasks) => 
            Repository.AddToUser(user._dataBaseModel, from userTask in userTasks select userTask._dataBaseModel);
    }
}