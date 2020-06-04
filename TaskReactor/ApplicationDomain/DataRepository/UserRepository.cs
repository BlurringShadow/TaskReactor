using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataRepository
{
    sealed class UserRepository : Repository<User, TaskReactorDbContext>, IUserRepository
    {
        [ImportingConstructor]
        public UserRepository([NotNull] TaskReactorDbContext dbContext) : base(dbContext)
        {
        }

        public void Register(User user) => Update(user);

        public async Task<User> LogInAsync(User user) => await LogInAsync(user, CancellationToken.None);

        public async Task<User> LogInAsync(User user, CancellationToken token)
        {
            var found = await FindByKeysAsync(token, user.Id);
            return found?.Password == user.Password ? found : null;
        }

        public void LogOff(User user) => Remove(user);
    }
}