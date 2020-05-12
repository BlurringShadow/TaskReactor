﻿using System.ComponentModel.Composition;
using ApplicationDomain.Models.DataBase;
using ApplicationDomain.Models.DataBase.Entity;
using JetBrains.Annotations;
using Utilities;

namespace ApplicationDomain.Repository
{
    public class UserRepository : Repository<User, TaskReactorDbContext>
    {
        [ImportingConstructor]
        public UserRepository([NotNull] TaskReactorDbContext dbContext) : base(dbContext)
        {
        }

        public async void Register([NotNull] User user)
        {
            await DbSet.AddAsync(user);
            await Context.SaveChangesAsync()!;
        }

        public async void LogOff([NotNull] User user)
        {
            DbSet.Remove(user);
            await Context.SaveChangesAsync()!;
        }
    }
}