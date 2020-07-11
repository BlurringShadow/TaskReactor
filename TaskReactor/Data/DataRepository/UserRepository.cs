using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Database;
using Data.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Data.DataRepository
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    sealed class UserRepository : Repository<User, TaskReactorDbContext>, IUserRepository
    {
        [ImportingConstructor]
        public UserRepository([NotNull] TaskReactorDbContext dbContext) : base(dbContext)
        {
        }

        public void Register(User user) => Update(user);

        public async Task<User> LogInAsync(User user) => await LogInAsync(user, CancellationToken.None);

        public async Task<User> LogInAsync(User user, CancellationToken token) =>
            await DbSet.Where(u => u.Id == user.Id && u.Password == user.Password).SingleOrDefaultAsync(token)!;

        public void LogOff(User user) => Remove(user);
    }
}