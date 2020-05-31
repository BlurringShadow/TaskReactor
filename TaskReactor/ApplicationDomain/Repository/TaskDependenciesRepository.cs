using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.Repository
{
    [Export]
    public class TaskDependenciesRepository : Repository<TaskDependency, TaskReactorDbContext>
    {
        [NotNull] private readonly
            Func<DbContext, UserTask, CancellationToken, Task<List<TaskDependency>>> _getDependenciesQuery;

        [ImportingConstructor]
        public TaskDependenciesRepository([NotNull] TaskReactorDbContext context) : base(context)
        {
            _getDependenciesQuery = EF.CompileAsyncQuery(
                (DbContext ctx, UserTask task, CancellationToken token) => ctx.Set<TaskDependency>()!
                    .Where(dependency => dependency.Target.Id == task.Id).ToListAsync(token).Result
            )!;
        }

        [NotNull]
        public async Task<List<TaskDependency>> GetDependencies([NotNull] UserTask task) =>
            await GetDependencies(task, CancellationToken.None)!;

        [NotNull]
        public async Task<List<TaskDependency>> GetDependencies([NotNull] UserTask task, CancellationToken token) =>
            await _getDependenciesQuery(Context, task, token)!;

        public void AddDependencies(
            [NotNull] UserTask target,
            [NotNull, ItemNotNull] params UserTask[] userTasks
        ) => AddDependencies(target, (IEnumerable<UserTask>)userTasks);

        public void AddDependencies(
            [NotNull] UserTask target,
            [NotNull, ItemNotNull] IEnumerable<UserTask> userTasks
        ) => Update(from task in userTasks select new TaskDependency {Target = target, Dependency = task});
    }
}