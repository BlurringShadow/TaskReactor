#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 23
// Time: 下午 10:43

#endregion

using System;
using System.Collections.Generic;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataModel;

namespace ApplicationDomain.ModelService
{
    abstract class ScheduleModelNotificationService<TScheduleModel, TItem> :
        ScheduleNotificationService<TScheduleModel>,
        IScheduleModelNotificationService<TScheduleModel, TItem>
        where TScheduleModel : ScheduleModel<TItem>
        where TItem : class, ISchedule, new()
    {
        protected ScheduleModelNotificationService(IEqualityComparer<TScheduleModel> comparer) : base(comparer)
        {
        }

        protected override IEnumerable<DateTime> Configuration(TScheduleModel model)
        {
            for (var i = 0;; ++i)
            {
                var nextDue = model[i];
                if (nextDue > model.EndTime) break;
                yield return nextDue;
            }
        }
    }
}