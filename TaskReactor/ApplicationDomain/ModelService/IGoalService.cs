﻿using System;
using System.Collections.Generic;
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
    [InheritedExport]
    public interface IGoalService : IService<Goal, TaskReactorDbContext, IGoalRepository, GoalModel>
    {
        /// <summary>
        /// Set notify action for user task
        /// </summary>
        Action<GoalModel> NotifyAction { get; set; }

        /// <summary>
        /// Get all goals from task
        /// </summary>
        /// <param name="task"> Input task </param>
        /// <returns> Async result of list goals </returns>
        [NotNull, ItemNotNull]
        Task<List<GoalModel>> GetAllFromTaskAsync([NotNull] UserTaskModel task);

        /// <summary>
        /// Get all goals from task
        /// </summary>
        /// <param name="task"> Input task </param>
        /// <param name="token"> <see cref="CancellationToken"/> </param>
        /// <returns> Async result of list goals </returns>
        [NotNull, ItemNotNull]
        Task<List<GoalModel>> GetAllFromTaskAsync([NotNull] UserTaskModel task, CancellationToken token);

        /// <summary>
        /// Add goals to task
        /// </summary>
        void AddToTask([NotNull] UserTaskModel userTask, [NotNull, ItemNotNull] params GoalModel[] goals);

        /// <summary>
        /// Add goals to task
        /// </summary>
        void AddToTask([NotNull] UserTaskModel userTask, [NotNull, ItemNotNull] IEnumerable<GoalModel> goals);
    }
}