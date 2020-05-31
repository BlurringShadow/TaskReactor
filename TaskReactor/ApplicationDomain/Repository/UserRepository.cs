using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.Repository
{
    [Export]
    public sealed class UserRepository : Repository<User, TaskReactorDbContext>
    {
        [ImportingConstructor]
        public UserRepository([NotNull] TaskReactorDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Register a uer
        /// </summary>
        /// <param name="user"> Id will be reassigned after finishing update </param>
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