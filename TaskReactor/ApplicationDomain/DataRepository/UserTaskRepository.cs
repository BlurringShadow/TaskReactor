using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.DataRepository
{
    sealed class UserTaskRepository : Repository<UserTask, TaskReactorDbContext>, IUserTaskRepository
    {
        [NotNull] private readonly Func<DbContext, User, CancellationToken, Task<List<UserTask>>> _getAllFromUserQuery;

        [ImportingConstructor]
        public UserTaskRepository([NotNull] TaskReactorDbContext context) : base(context) =>
            _getAllFromUserQuery = EF.CompileAsyncQuery(
                (DbContext ctx, User user, CancellationToken token) =>
                    ctx.Set<UserTask>()!.Where(task => task.OwnerUser.Id == user.Id).ToList()
            )!;

        public async Task<List<UserTask>> GetAllFromUserAsync(User user) =>
            await _getAllFromUserQuery(Context, user, CancellationToken.None)!;

        public async Task<List<UserTask>> GetAllFromUserAsync(User user, CancellationToken token) =>
            await _getAllFromUserQuery(Context, user, token)!;
    }
}