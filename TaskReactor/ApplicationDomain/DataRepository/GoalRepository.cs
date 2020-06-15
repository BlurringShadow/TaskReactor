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
        [ImportingConstructor]
        public GoalRepository([NotNull] TaskReactorDbContext context) : base(context)
        {
        }

        public async Task<List<Goal>> GetAllFromTaskAsync(UserTask task) =>
            await GetAllFromTaskAsync(task, CancellationToken.None);

        public async Task<List<Goal>> GetAllFromTaskAsync(UserTask task, CancellationToken token) =>
            (await Task.Run(
                () =>
                {
                    lock(Context)
                        return DbSet!.Include(goal => goal.FromTask)!
                            .Where(goal => task.Id == goal.FromTask.Id).ToList()!;
                }, 
                token
            ))!;

        public void AddToTask(UserTask userTask, params Goal[] goals) =>
            AddToTask(userTask, (IEnumerable<Goal>)goals);

        public void AddToTask(UserTask userTask, IEnumerable<Goal> goals)
        {
            userTask.Goals ??= new List<Goal>();
            foreach(var goal in goals)
            {
                // ReSharper disable once PossibleNullReferenceException
                goal.FromTask = userTask;
                userTask.Goals.Add(goal);
            }

            lock(Context) Context.Update(userTask);
        }
    }
}