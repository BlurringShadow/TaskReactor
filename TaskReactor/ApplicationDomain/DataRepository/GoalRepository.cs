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

namespace ApplicationDomain.DataRepository
{
    sealed class GoalRepository : Repository<Goal, TaskReactorDbContext>, IGoalRepository
    {
        [NotNull] private readonly Func<DbContext, UserTask, CancellationToken, Task<List<Goal>>> _getAllFromTaskQuery;

        [ImportingConstructor]
        public GoalRepository([NotNull] TaskReactorDbContext context) : base(context)
        {
            _getAllFromTaskQuery = EF.CompileAsyncQuery(
                (DbContext ctx, UserTask task, CancellationToken token) =>
                    ctx.Set<Goal>()!.Where(goal => task.Id == goal.FromTask.Id).ToList()
            )!;
        }

        public async Task<List<Goal>> GetAllFromTaskAsync(UserTask task) =>
            await GetAllFromTaskAsync(task, CancellationToken.None);

        public async Task<List<Goal>> GetAllFromTaskAsync(UserTask task, CancellationToken token) =>
            (await _getAllFromTaskQuery(Context, task, token)!)!;

        public void AddToTask(UserTask userTask, params Goal[] goals) =>
            AddToTask(userTask, (IEnumerable<Goal>)goals);

        public void AddToTask(UserTask userTask, IEnumerable<Goal> goals)
        {
            foreach (var goal in goals)
            {
                goal.FromTask = userTask;
                Update(goal);
            }
        }
    }
}