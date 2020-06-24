#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 24
// Time: 上午 11:52

#endregion

using System.ComponentModel.Composition;
using ApplicationDomain.DataModel;

namespace ApplicationDomain.ModelService
{
    [InheritedExport]
    interface IUserTaskModelNotificationService : IScheduleNotificationService<UserTaskModel>
    {
    }
}