#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 24
// Time: 上午 12:07

#endregion

using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Coravel.Scheduling.Schedule.Interfaces;
using JetBrains.Annotations;

namespace ApplicationDomain.ModelService
{
    /// <summary>
    /// Provide task scheduler services
    /// </summary>
    [InheritedExport]
    interface ISchedulerService : IAsyncDisposable
    {
        IScheduledEventConfiguration AddSchedule(
            [NotNull] string id,
            [NotNull] Func<Task> action,
            [NotNull] Func<IScheduleInterval, IScheduledEventConfiguration> configuration
        );

        void AddOnceSchedule([NotNull] string id, [NotNull] Func<Task> action, DateTime date);

        bool Contains([NotNull] string id);

        bool Remove([NotNull] string id);
    }
}