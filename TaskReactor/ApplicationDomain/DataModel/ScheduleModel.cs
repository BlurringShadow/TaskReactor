using System;
using System.ComponentModel;
using Data.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataModel
{
    public abstract class ScheduleModel<T> : ItemModel<T> where T : class, ISchedule, new()
    {
        public override DateTime StartTime
        {
            get => base.StartTime;
            set
            {
                base.StartTime = value;
                NotifyOfPropertyChange(nameof(StartTimeOfDay));
                NotifyOfPropertyChange(nameof(EndTimeOfDay));
            }
        }

        public virtual TimeSpan DurationOfOneTime
        {
            get => _dataBaseModel.DurationOfOneTime;
            set
            {
                _dataBaseModel.DurationOfOneTime = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(StartTimeOfDay));
                NotifyOfPropertyChange(nameof(EndTimeOfDay));
            }
        }

        public TimeSpan StartTimeOfDay => _dataBaseModel.StartTime.TimeOfDay;

        public TimeSpan EndTimeOfDay => StartTimeOfDay + DurationOfOneTime;

        [NotNull] public virtual Interval Interval
        {
            get => _dataBaseModel.Interval;
            set
            {
                ResetIntervalKind(value.Kind);
                _dataBaseModel.Interval = value;
                NotifyOfPropertyChange();
            }
        }

        void ResetIntervalKind(IntervalKind kind)
        {
            _intervalImpl = kind switch
            {
                IntervalKind.YearByWeek => count => StartTime.AddDays(-StartTime.Day)
                    .AddYears(count)
                    .AddWeeks(StartTime.WeekOfMonth())
                    .AddDays((double)StartTime.DayOfWeek),
                IntervalKind.YearByDay => count => StartTime.AddYears(count),
                IntervalKind.MonthByWeek => count => StartTime.AddDays(-StartTime.Day)
                    .AddMonths(count)
                    .AddWeeks(StartTime.WeekOfMonth())
                    .AddDays((double)StartTime.DayOfWeek),
                IntervalKind.MonthByDay => count => StartTime.AddMonths(count),
                IntervalKind.ByWeek => count => StartTime.AddWeeks(count),
                IntervalKind.ByDay => count => StartTime.AddDays(count),
                _ => throw new InvalidEnumArgumentException(
                    nameof(Interval.Kind), (byte)Interval.Kind, typeof(IntervalKind)
                )
            };
        }

        Func<int, DateTime> _intervalImpl;

        /// <summary>
        /// Get the date time using specified interval count.
        /// Implementation for <see cref="Data.Database.Entity.Interval"/>
        /// </summary>
        /// <param name="count">Interval count</param>
        /// <returns> Next </returns>
        public DateTime this[int count] => _intervalImpl!(count);

        protected ScheduleModel([NotNull] T item) : base(item) => ResetIntervalKind(_dataBaseModel.Interval.Kind);

        protected ScheduleModel()
        {
        }
    }
}