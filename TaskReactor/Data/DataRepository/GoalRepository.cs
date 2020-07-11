using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Data.Database;
using Data.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Data.DataRepository
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    sealed class GoalRepository : Repository<Goal, TaskReactorDbContext>, IGoalRepository
    {
        [ImportingConstructor]
        public GoalRepository([NotNull] TaskReactorDbContext context) : base(context)
        {
        }

        public async Task<IList<Goal>> GetAllFromTaskAsync(UserTask task) =>
            await GetAllFromTaskAsync(task, CancellationToken.None);

        public async Task<IList<Goal>> GetAllFromTaskAsync(UserTask task, CancellationToken token) =>
            (await Context.Set<UserTask>()!.Include(t => t.Goals)!
                .SingleAsync(t => t.Id == task.Id, token)!)!.Goals!;

        public void AddToTask(UserTask userTask, params Goal[] goals) =>
            AddToTask(userTask, (IEnumerable<Goal>)goals);

        public void AddToTask(UserTask userTask, IEnumerable<Goal> goals)
        {
            userTask.Goals ??= new List<Goal>();
            foreach (var goal in goals)
            {
                // ReSharper disable once PossibleNullReferenceException
                goal.FromTask = userTask;
                userTask.Goals.Add(goal);
            }

            Context.Update(userTask);
        }
    }
}