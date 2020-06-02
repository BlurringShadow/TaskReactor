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
    [InheritedExport]
    public interface IUserTaskService : IService<UserTask, TaskReactorDbContext, IUserTaskRepository, UserTaskModel>
    {
        /// <summary>
        /// Get all tasks from user
        /// </summary>
        /// <param name="user"> The user </param>
        /// <returns> Async result contains list of <see cref="UserTask"/> </returns>
        [NotNull]
        Task<List<UserTask>> GetAllFromUserAsync([NotNull] UserModel user);

        /// <summary>
        /// Get all tasks from user
        /// </summary>
        /// <param name="user"> The user </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Async result contains list of <see cref="UserTask"/> </returns>
        [NotNull]
        Task<List<UserTask>> GetAllFromUserAsync([NotNull] UserModel user, CancellationToken token);
    }
}