#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 21
// Time: 上午 11:57

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ApplicationDomain.ModelService
{
    abstract class ScheduleNotificationService<TModel> : IScheduleNotificationService<TModel>
    {
        private class TimerEvent : IAsyncDisposable
        {
            [NotNull] readonly Timer _timer;

            [NotNull] public readonly Action<TModel> NotifyAction;

            IEnumerator<DateTime> _enumerator;

            public TimerEvent(
                [NotNull] IEnumerable<DateTime> enumerable,
                [NotNull] Action<TModel> notifyAction,
                [NotNull] TModel model
            )
            {
                _timer = new Timer(Callback, model, -1, -1);
                _enumerator = enumerable.GetEnumerator();
                NotifyAction = notifyAction;
            }

            void Callback(object state)
            {
                if (_enumerator?.MoveNext() == false || _enumerator is null) return;
                var span = DateTime.Now - _enumerator.Current;

                // Filter time that less than zero
                if (span < TimeSpan.Zero) return;

                NotifyAction((TModel)state);

                _timer.Change(span, TimeSpan.FromMilliseconds(-1));
            }

            public ValueTask DisposeAsync() => _timer.DisposeAsync();
        }

        [NotNull] readonly IDictionary<TModel, TimerEvent> _timerEvents;

        public IReadOnlyCollection<TModel> Models => _timerEvents.Keys.ToArray();

        protected ScheduleNotificationService(IEqualityComparer<TModel> comparer = null) =>
            _timerEvents = new ConcurrentDictionary<TModel, TimerEvent>(comparer);

        public bool ContainsModel(TModel t) => _timerEvents.ContainsKey(t);

        public void AddModelAction(TModel t, Action<TModel> notifyAction) =>
            _timerEvents.Add(t, new TimerEvent(Configuration(t), notifyAction, t));

        public void UpdateModelAction(TModel t, Action<TModel> notifyAction)
        {
            if (ContainsModel(t)) _timerEvents.Remove(t);
            AddModelAction(t, notifyAction);
        }

        public void UpdateModel(TModel t)
        {
            var timerEvent = _timerEvents[t];
            var previousAction = timerEvent!.NotifyAction;
            timerEvent.DisposeAsync();
            _timerEvents.Remove(t);
            _timerEvents.Add(t, new TimerEvent(Configuration(t), previousAction, t));
        }

        public void RemoveModel(TModel t) => _timerEvents.Remove(t);

        /// <summary>
        /// Configure schedule way. <para/>
        /// Function return a <see cref="DateTime"/> that next time to execute.
        /// </summary>
        /// <returns> Next due time <see cref="DateTime"/> </returns>
        [NotNull]
        protected abstract IEnumerable<DateTime> Configuration([NotNull] TModel model);
    }
}