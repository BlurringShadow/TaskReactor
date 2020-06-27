﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Database;
using Data.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Data.DataRepository
{
    sealed class UserTaskRepository : Repository<UserTask, TaskReactorDbContext>, IUserTaskRepository
    {
        [ImportingConstructor]
        public UserTaskRepository([NotNull] TaskReactorDbContext context) : base(context)
        {
        }

        public async Task<IList<UserTask>> GetAllFromUserAsync(User user) =>
            await GetAllFromUserAsync(user, CancellationToken.None);

        public async Task<IList<UserTask>> GetAllFromUserAsync(User user, CancellationToken token) =>
            (await Task.Run(
                () =>
                {
                    lock (Context)
                        return Context.Set<User>()!.Include(u => u.Tasks)!.First(u => u.Id == user.Id)!.Tasks;
                }, token
            ))!;

        public void AddToUser(User user, params UserTask[] userTasks) =>
            AddToUser(user, (IEnumerable<UserTask>)userTasks);

        public void AddToUser(User user, IEnumerable<UserTask> userTasks)
        {
            user.Tasks ??= new List<UserTask>();

            foreach (var userTask in userTasks)
            {
                // ReSharper disable once PossibleNullReferenceException
                userTask.OwnerUser = user;
                user.Tasks.Add(userTask);
            }

            Context.Update(user);
        }
    }
}