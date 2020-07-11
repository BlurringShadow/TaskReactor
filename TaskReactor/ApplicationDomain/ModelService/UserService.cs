using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using Data.Database;
using Data.Database.Entity;
using Data.DataRepository;
using JetBrains.Annotations;

namespace ApplicationDomain.ModelService
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    sealed class UserService : Service<User, TaskReactorDbContext, IUserRepository, UserModel>, IUserService
    {
        [ImportingConstructor]
        public UserService([NotNull] IUserRepository repository) : base(repository)
        {
        }

        public void Register(UserModel user) => Repository.Register(user._dataBaseModel);

        public async Task<UserModel> LogInAsync(int userId, string userPassword) =>
            await LogInAsync(userId, userPassword, CancellationToken.None);

        public async Task<UserModel> LogInAsync(int userId, string userPassword, CancellationToken token) =>
            CreateModelInstance(
                (await Repository.LogInAsync(
                    new User { Id = userId, Password = userPassword }, CancellationToken.None
                ))!
            );

        public void LogOff(UserModel user) => Repository.LogOff(user._dataBaseModel);
    }
}