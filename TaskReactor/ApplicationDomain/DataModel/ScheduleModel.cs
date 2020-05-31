using System;
using System.ComponentModel;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataModel
{
    public abstract class ScheduleModel<T> : ItemModel<T> where T : Schedule, new()
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
                _dataBaseModel.Interval = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Get the date time using specified interval count.
        /// Implementation for <see cref="Database.Entity.Interval"/>
        /// </summary>
        /// <param name="count">Interval count</param>
        /// <returns></returns>
        public DateTime this[byte count] =>
            Interval.Kind switch
            {
                IntervalKind.YearByWeek => StartTime.AddDays(-StartTime.Day)
                    .AddYears(count)
                    .AddWeeks(StartTime.WeekOfMonth())
                    .AddDays((double)StartTime.DayOfWeek),
                IntervalKind.YearByDay => StartTime.AddYears(count),
                IntervalKind.MonthByWeek => StartTime.AddDays(-StartTime.Day)
                    .AddMonths(count)
                    .AddWeeks(StartTime.WeekOfMonth())
                    .AddDays((double)StartTime.DayOfWeek),
                IntervalKind.MonthByDay => StartTime.AddMonths(count),
                IntervalKind.ByWeek => StartTime.AddWeeks(count),
                IntervalKind.ByDay => StartTime.AddDays(count),
                _ => throw new InvalidEnumArgumentException(
                    nameof(Interval.Kind), (byte)Interval.Kind, typeof(IntervalKind)
                )
            };

        protected ScheduleModel([NotNull] T item) : base(item)
        {
        }
    }
}