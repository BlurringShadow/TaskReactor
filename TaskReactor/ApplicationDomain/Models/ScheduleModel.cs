using System;
using System.ComponentModel;
using ApplicationDomain.Models.DataBase.Entity;
using JetBrains.Annotations;
using Utilities;

namespace ApplicationDomain.Models
{
    public class ScheduleModel : Model<Schedule>
    {
        public User OwnerUser => _dataBaseModel.OwnerUser;

        [NotNull] public string Title
        {
            get => _dataBaseModel.Title;
            set
            {
                _dataBaseModel.Title = value;
                NotifyOfPropertyChange();
            }
        }

        public DateTime StartTime
        {
            get => _dataBaseModel.StartTime;
            set
            {
                _dataBaseModel.StartTime = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(StartTimeOfDay));
                NotifyOfPropertyChange(nameof(EndTimeOfDay));
            }
        }

        public TimeSpan DurationOfOneTime
        {
            get => _dataBaseModel.DurationOfOneTime;
            set
            {
                _dataBaseModel.DurationOfOneTime = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(EndTimeOfDay));
            }
        }

        public DateTime EndTime
        {
            get => _dataBaseModel.EndTime;
            set
            {
                _dataBaseModel.EndTime = value;
                NotifyOfPropertyChange();
            }
        }

        public TimeSpan StartTimeOfDay => _dataBaseModel.StartTimeOfDay;

        public TimeSpan EndTimeOfDay => _dataBaseModel.EndTimeOfDay;

        [NotNull] public Interval Interval
        {
            get => _dataBaseModel.Interval;
            set
            {
                _dataBaseModel.Interval = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Get the date time using specified interval value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DateTime this[byte value] =>
            Interval.Kind switch
            {
                IntervalKind.YearByWeek => StartTime.AddDays(-StartTime.Day)
                    .AddYears(value)
                    .AddWeeks(StartTime.WeekOfMonth())
                    .AddDays((double)StartTime.DayOfWeek),
                IntervalKind.YearByDay => StartTime.AddYears(value),
                IntervalKind.MonthByWeek => StartTime.AddDays(-StartTime.Day)
                    .AddMonths(value)
                    .AddWeeks(StartTime.WeekOfMonth())
                    .AddDays((double)StartTime.DayOfWeek),
                IntervalKind.MonthByDay => StartTime.AddMonths(value),
                IntervalKind.ByWeek => StartTime.AddWeeks(value),
                IntervalKind.ByDay => StartTime.AddDays(value),
                _ => throw new InvalidEnumArgumentException(
                    nameof(Interval.Kind), (byte)Interval.Kind, typeof(IntervalKind)
                )
            };

        public ScheduleModel([NotNull] Schedule dataBaseModel) : base(dataBaseModel)
        {
        }
    }
}