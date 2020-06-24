#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 23
// Time: 下午 10:43

#endregion

using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataModel;

namespace ApplicationDomain.ModelService
{
    /// <summary>
    /// Provide notification service for <see cref="ScheduleModel{T}"/>
    /// </summary>
    interface IScheduleModelNotificationService<TScheduleModel, TItem> : IScheduleNotificationService<TScheduleModel>
        where TScheduleModel : ScheduleModel<TItem>
        where TItem : class, ISchedule, new()
    {
    }
}