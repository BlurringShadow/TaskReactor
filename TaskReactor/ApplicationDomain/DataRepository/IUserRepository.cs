using System.ComponentModel.Composition;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database;

namespace ApplicationDomain.DataRepository
{
    [InheritedExport]
    public interface IUserRepository : IRepository<User, TaskReactorDbContext>
    {
        /// <summary>
        /// Register a uer
        /// </summary>
        /// <param name="user"> Id will be reassigned after finishing update </param>
        void Register([NotNull] User user);

        /// <summary>
        /// User log in
        /// </summary>
        /// <returns> User which matched input user's id and password </returns>
        [NotNull] 
        Task<User> LogInAsync([NotNull] User user);

        /// <summary>
        /// User log in
        /// </summary>
        /// <returns> User which matched input user's id and password </returns>
        [NotNull] 
        Task<User> LogInAsync([NotNull] User user, CancellationToken token);

        /// <summary>
        /// Delete the user
        /// </summary>
        void LogOff([NotNull] User user);
    }
}