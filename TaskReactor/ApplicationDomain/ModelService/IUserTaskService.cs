using System;
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
    public interface IUserTaskService : IService<UserTask, TaskReactorDbContext, IUserTaskRepository, UserTaskModel>
    {
        /// <summary>
        /// Set notify action for user task
        /// </summary>
        Action<UserTaskModel> NotifyAction { get; set; }

        /// <summary>
        /// Get all tasks from user
        /// </summary>
        /// <param name="user"> The user </param>
        /// <returns> Async result contains list of <see cref="Data.Database.Entity.UserTask"/> </returns>
        [NotNull, ItemNotNull]
        Task<IList<UserTaskModel>> GetAllFromUserAsync([NotNull] UserModel user);

        /// <summary>
        /// Get all tasks from user
        /// </summary>
        /// <param name="user"> The user </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Async result contains list of <see cref="Data.Database.Entity.UserTask"/> </returns>
        [NotNull, ItemNotNull]
        Task<IList<UserTaskModel>> GetAllFromUserAsync([NotNull] UserModel user, CancellationToken token);

        /// <summary>
        /// Add tasks to user
        /// </summary>
        void AddToUser([NotNull] UserModel user,
            [NotNull, ItemNotNull] params UserTaskModel[] userTasks);

        /// <summary>
        /// Add tasks to user
        /// </summary>
        void AddToUser([NotNull] UserModel user,
            [NotNull, ItemNotNull] IEnumerable<UserTaskModel> userTasks);
    }
}