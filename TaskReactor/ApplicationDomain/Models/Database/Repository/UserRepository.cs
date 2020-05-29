using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Models.Database.Entity;
using ApplicationDomain.Repository;
using JetBrains.Annotations;

namespace ApplicationDomain.Models.Database.Repository
{
    [Export]
    public sealed class UserRepository : Repository<User, TaskReactorDbContext>
    {
        [ImportingConstructor]
        public UserRepository([NotNull] TaskReactorDbContext dbContext) : base(dbContext)
        {
        }

        public void Register([NotNull] User user) => Update(user);

        public async Task<User> LogInAsync([NotNull] User user) => await LogInAsync(user, CancellationToken.None);

        public async Task<User> LogInAsync([NotNull] User user, CancellationToken token)
        {
            var found = await DbSet.FindAsync(new object[] {user.Id}, token);
            return found?.Password == user.Password ? found : null;
        }

        public void LogOff([NotNull] User user) => Remove(user);
    }
}