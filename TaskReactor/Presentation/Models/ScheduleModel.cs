using System;
using JetBrains.Annotations;
using TaskReactor.Utilities;

namespace TaskReactor.Models
{
    public class ScheduleModel : Model<Schedule>
    {
        public User OwnerUser => _dataBaseModel.OwnerUser;

        [NotNull] 
        public string Title
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

        public TimeSpan StartTimeOfDay => StartTime.TimeOfDay;

        public TimeSpan EndTimeOfDay => StartTimeOfDay + DurationOfOneTime;

        public ScheduleModel([NotNull] Schedule dataBaseModel) : base(dataBaseModel)
        {
        }
    }
}