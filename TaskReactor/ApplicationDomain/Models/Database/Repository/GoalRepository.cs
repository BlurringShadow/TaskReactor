using System.ComponentModel.Composition;
using ApplicationDomain.Models.Database.Entity;
using ApplicationDomain.Repository;
using JetBrains.Annotations;

namespace ApplicationDomain.Models.Database.Repository
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