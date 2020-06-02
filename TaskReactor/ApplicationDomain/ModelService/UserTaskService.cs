using System.Collections.Generic;
using System.ComponentModel.Composition;
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

        /// <summary>
        /// Get all tasks from user
        /// </summary>
        /// <param name="user"> The user </param>
        /// <returns> Async result contains list of <see cref="UserTask"/> </returns>
        public async Task<List<UserTask>> GetAllFromUserAsync(UserModel user) =>
            await GetAllFromUserAsync(user, CancellationToken.None);

        /// <summary>
        /// Get all tasks from user
        /// </summary>
        /// <param name="user"> The user </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Async result contains list of <see cref="UserTask"/> </returns>
        public async Task<List<UserTask>> GetAllFromUserAsync(UserModel user, CancellationToken token) =>
            await Repository.GetAllFromUserAsync(user._dataBaseModel, token);
    }
}