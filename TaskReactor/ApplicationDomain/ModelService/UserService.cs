using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataModel;
using ApplicationDomain.DataRepository;
using JetBrains.Annotations;

namespace ApplicationDomain.ModelService
{
    sealed class UserService : Service<User, TaskReactorDbContext, IUserRepository, UserModel>, IUserService
    {
        [ImportingConstructor]
        public UserService([NotNull] IUserRepository repository) : base(repository)
        {
        }

        public void Register(UserModel user) => Repository.Register(user._dataBaseModel);

        public async Task<User> LogInAsync(UserModel user) =>
            await Repository.LogInAsync(user._dataBaseModel, CancellationToken.None);

        public async Task<User> LogInAsync(UserModel user, CancellationToken token) =>
            await Repository.LogInAsync(user._dataBaseModel, token);

        public void LogOff(UserModel user) => Repository.LogOff(user._dataBaseModel);
    }
}