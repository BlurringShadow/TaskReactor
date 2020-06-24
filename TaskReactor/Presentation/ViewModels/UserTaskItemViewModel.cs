#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 09
// Time: 上午 11:49

#endregion

using System;
using System.Globalization;
using ApplicationDomain.DataModel;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    public sealed class UserTaskItemViewModel : Screen
    {
        [NotNull] private UserTaskModel _model;

        [NotNull] public UserTaskModel Model
        {
            get => _model;
            set
            {
                Set(ref _model, value);
                NotifyOfPropertyChange(nameof(DisplayName));
                NotifyOfPropertyChange(nameof(TaskStartTime));
            }
        }

        [NotNull] public override string DisplayName => Model.Title;

        [NotNull] public string TaskStartTime => Model.StartTime.ToString(CultureInfo.CurrentCulture);

        public event Action<UserTaskItemViewModel> OnClickEvent;

        public event Action<UserTaskItemViewModel> OnRemoveEvent;

        // ReSharper disable once NotNullMemberIsNotInitialized
        public UserTaskItemViewModel([NotNull] UserTaskModel model) => Model = model;

        public void OnClick() => OnClickEvent?.Invoke(this);

        public void OnRemove() => OnRemoveEvent?.Invoke(this);
    }
}