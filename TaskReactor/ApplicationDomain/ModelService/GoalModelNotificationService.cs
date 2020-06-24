#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 24
// Time: 下午 12:19

#endregion

using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataModel;

namespace ApplicationDomain.ModelService
{
    class GoalModelNotificationService : 
        ScheduleModelNotificationService<GoalModel, Goal>,
        IGoalModelNotificationService
    {
    }
}