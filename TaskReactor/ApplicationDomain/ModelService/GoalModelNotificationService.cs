#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 24
// Time: 下午 12:19

#endregion

using System.Collections.Generic;
using ApplicationDomain.DataModel;
using Data.Database.Entity;

namespace ApplicationDomain.ModelService
{
    class GoalModelNotificationService :
        ScheduleModelNotificationService<GoalModel, Goal>,
        IGoalModelNotificationService
    {
        private class GoalModelComparer : IEqualityComparer<GoalModel>
        {
            public bool Equals(GoalModel x, GoalModel y) =>
                x?.Identity == y?.Identity;

            public int GetHashCode(GoalModel obj) =>
                obj.Identity.GetHashCode();
        }

        public GoalModelNotificationService() : base(new GoalModelComparer())
        {
        }
    }
}