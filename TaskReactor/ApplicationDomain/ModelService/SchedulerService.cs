#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 22
// Time: 下午 6:24

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coravel;
using Coravel.Scheduling.Schedule;
using Coravel.Scheduling.Schedule.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace ApplicationDomain.ModelService
{
    sealed class SchedulerService : ISchedulerService
    {
        [NotNull] readonly IHost _host;

        [NotNull] readonly ISet<string> _set = new SortedSet<string>();

        [NotNull] public Scheduler _scheduler
        {
            get
            {
                Scheduler scheduler = null;
                _host.Services.UseScheduler(s => scheduler = (Scheduler)s);
                return scheduler;
            }
        }

        public SchedulerService()
        {
            _host = new HostBuilder()
                    .ConfigureServices(services => services.AddScheduler())
                !.Build()!;

            // ReSharper disable once PossibleNullReferenceException
            _host.StartAsync();
        }

        public IScheduledEventConfiguration AddSchedule(
            string id,
            Func<Task> action,
            Func<IScheduleInterval, IScheduledEventConfiguration> configuration
        )
        {
            if (Contains(id)) return null;
            _set.Add(id);
            return configuration(_scheduler.ScheduleAsync(action))!.PreventOverlapping(id);
        }

        public void AddOnceSchedule(string id, Func<Task> action, DateTime date) =>
            AddSchedule(
                id, async () =>
                {
                    var task = action();
                    if (!(task is null)) await task;
                    Remove(id);
                }, interval => interval?.DailyAt(date.Hour, date.Minute)
            );

        public bool Contains(string id) => _set.Contains(id);

        public bool Remove(string id)
        {
            if (!_set.Remove(id)) return false;

            _scheduler.TryUnschedule(id);
            return true;
        }

        public async ValueTask DisposeAsync()
        {
            var task = _host.StopAsync();
            if (!(task is null)) await task;
            _host.Dispose();
        }
    }
}