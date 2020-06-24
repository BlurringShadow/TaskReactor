#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 23
// Time: 下午 10:39

#endregion

using System;
using System.Collections.Generic;
using ApplicationDomain.DataModel;

namespace ApplicationDomain.ModelService
{
    /// <summary>
    /// Provide notification service for <see cref="UserTaskModel"/>
    /// </summary>
    class UserTaskModelNotificationService : 
        ScheduleNotificationService<UserTaskModel>,
        IUserTaskModelNotificationService
    {
        private class UserTaskModelComparer : IEqualityComparer<UserTaskModel>
        {
            public bool Equals(UserTaskModel x, UserTaskModel y) => x?.Identity == y?.Identity;

            public int GetHashCode(UserTaskModel obj) => obj.Identity.GetHashCode();
        }

        public UserTaskModelNotificationService() : base(new UserTaskModelComparer())
        {
        }

        protected override IEnumerable<DateTime> Configuration(UserTaskModel model)
        {
            yield return model.StartTime;
        }
    }
}