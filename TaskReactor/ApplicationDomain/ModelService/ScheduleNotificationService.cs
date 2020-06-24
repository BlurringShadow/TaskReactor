﻿#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 21
// Time: 上午 11:57

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ApplicationDomain.ModelService
{
    abstract class ScheduleNotificationService<TModel> : IScheduleNotificationService<TModel>
    {
        protected class TimerEvent : IAsyncDisposable
        {
            [NotNull] readonly Timer _timer;

            [NotNull] IEnumerable<DateTime> _nextTimeEnumerable;

            [NotNull] public IEnumerable<DateTime> NextTimeEnumerable
            {
                get => _nextTimeEnumerable;
                set
                {
                    _enumerator = value.GetEnumerator();
                    _nextTimeEnumerable = value;
                }
            }

            [NotNull] public Action NotifyAction;

            IEnumerator<DateTime> _enumerator;

            // ReSharper disable once NotNullMemberIsNotInitialized
            public TimerEvent([NotNull] IEnumerable<DateTime> enumerable, [NotNull] Action notifyAction)
            {
                _timer = new Timer(Callback);
                NextTimeEnumerable = enumerable;
                NotifyAction = notifyAction;
            }

            void Callback(object state)
            {
                if (_enumerator?.MoveNext() == false || _enumerator is null) return;

                NotifyAction();

                _timer.Change(DateTime.Now - _enumerator.Current, TimeSpan.FromMilliseconds(-1));
            }

            public ValueTask DisposeAsync() => _timer.DisposeAsync();
        }

        [NotNull] readonly IDictionary<TModel, TimerEvent> _timerEvents;

        protected ScheduleNotificationService(IEqualityComparer<TModel> comparer = null) =>
            _timerEvents = new ConcurrentDictionary<TModel, TimerEvent>(comparer);

        public bool ContainsModel(TModel t) => _timerEvents.ContainsKey(t);

        public void AddModelAction(TModel t, Action<TModel> notifyAction) =>
            _timerEvents.Add(t, new TimerEvent(Configuration(t), () => notifyAction(t)));

        public void UpdateModelAction(TModel t, Action<TModel> notifyAction)
        {
            if (ContainsModel(t)) _timerEvents.Remove(t);
            AddModelAction(t, notifyAction);
        }

        public void RemoveModel(TModel t) => _timerEvents.Remove(t);

        /// <summary>
        /// Configure schedule way
        /// </summary>
        [NotNull]
        protected abstract IEnumerable<DateTime> Configuration([NotNull] TModel model);
    }
}