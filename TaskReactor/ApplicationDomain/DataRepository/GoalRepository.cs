using System.ComponentModel.Composition;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataRepository
{
    [Export]
    public class GoalRepository : Repository<Goal, TaskReactorDbContext>
    {
        [ImportingConstructor]
        public GoalRepository([NotNull] TaskReactorDbContext context) : base(context)
        {
        }
    }
}