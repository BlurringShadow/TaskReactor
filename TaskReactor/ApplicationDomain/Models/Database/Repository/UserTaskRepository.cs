﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.Models.Database.Entity;
using ApplicationDomain.Repository;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.Models.Database.Repository
{
    [Export]
    public class UserTaskRepository : Repository<UserTask, TaskReactorDbContext>
    {
        [NotNull] private readonly Func<DbContext, User, CancellationToken, Task<List<UserTask>>> _getAllFromUserQuery;

        [ImportingConstructor]
        public UserTaskRepository([NotNull] TaskReactorDbContext context) : base(context)
        {
            _getAllFromUserQuery = EF.CompileAsyncQuery(
                (DbContext ctx, User user, CancellationToken token) => ctx.Set<UserTask>()!.Where(task => task.OwnerUser.Id == user.Id).ToListAsync(token).Result
            )!;
        }

        public async Task<List<UserTask>> GetAllFromUserAsync([NotNull] User user, CancellationToken token) => 
            await _getAllFromUserQuery(Context, user, token)!;
    }
}