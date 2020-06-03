﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.DataRepository
{
    [Export]
    public class TaskDependenciesRepository : Repository<TaskDependency, TaskReactorDbContext>,
        ITaskDependenciesRepository
    {
        [ImportingConstructor]
        public TaskDependenciesRepository([NotNull] TaskReactorDbContext context) : base(context)
        {
        }

        public async Task<List<TaskDependency>> GetDependenciesAsync(UserTask task) =>
            await GetDependenciesAsync(task, CancellationToken.None)!;

        public async Task<List<TaskDependency>> GetDependenciesAsync(UserTask task, CancellationToken token) =>
            (await Context.Set<TaskDependency>()!
                        .Include(d => d.Target)!
                    .Include(d => d.Dependency)!
                .Where(dependency => dependency.Target.Id == task.Id).ToListAsync(token)!)!;

        public IList<TaskDependency> AddDependencies(
            UserTask target,
            params UserTask[] userTasks
        ) => AddDependencies(target, (IEnumerable<UserTask>)userTasks);

        public IList<TaskDependency> AddDependencies(
            UserTask target,
            IEnumerable<UserTask> userTasks
        )
        {
            var taskDependencies =
                (from task in userTasks select new TaskDependency {Target = target, Dependency = task}).ToList();
            Update(taskDependencies);
            return taskDependencies;
        }
    }
}