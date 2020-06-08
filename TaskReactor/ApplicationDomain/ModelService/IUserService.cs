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
    [InheritedExport]
    public interface IUserService : IService<User, TaskReactorDbContext, IUserRepository, UserModel>
    {
        /// <summary>
        /// Register a uer
        /// </summary>
        /// <param name="user"> User id will be reassigned after finishing update </param>
        void Register([NotNull] UserModel user);

        /// <summary>
        /// User log in
        /// </summary>
        /// <returns> User model which matched input user's id and password </returns>
        [NotNull]
        Task<UserModel> LogInAsync(int userId, [NotNull] string userPassword);

        /// <summary>
        /// User log in
        /// </summary>
        /// <returns> User model which matched input user's id and password </returns>
        [NotNull]
        Task<UserModel> LogInAsync(int userId, [NotNull] string userPassword, CancellationToken token);

        /// <summary>
        /// Delete the user model
        /// </summary>
        void LogOff([NotNull] UserModel user);
    }
}