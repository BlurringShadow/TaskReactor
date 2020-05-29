using System.ComponentModel.Composition;
using ApplicationDomain.Models.Database;
using ApplicationDomain.Models.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.Repository
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