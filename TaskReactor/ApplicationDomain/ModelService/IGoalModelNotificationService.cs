#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 24
// Time: 下午 12:19

#endregion

using System.ComponentModel.Composition;
using ApplicationDomain.DataModel;
using Data.Database.Entity;

namespace ApplicationDomain.ModelService
{
    /// <summary>
    /// Provide notification service for <see cref="GoalModel"/>
    /// </summary>
    [InheritedExport]
    interface IGoalModelNotificationService : IScheduleModelNotificationService<GoalModel, Goal>
    {
    }
}