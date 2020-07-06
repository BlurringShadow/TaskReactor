#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 25
// Time: 下午 8:14

#endregion

using System;
using ApplicationDomain.DataModel;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels.UserProfile.Overview
{
    public sealed class GoalItemViewModel : Screen
    {
        [NotNull] GoalModel _goalModel;

        [NotNull] public GoalModel GoalModel
        {
            get => _goalModel;
            set
            {
                Set(ref _goalModel, value);
                NotifyOfPropertyChange(nameof(DisplayName));
            }
        }

        [NotNull] public override string DisplayName
        {
            get => GoalModel.Title;
            set
            {
                GoalModel.Title = value;
                base.DisplayName = value;
            }
        }

        public event Action<GoalItemViewModel> OnClickEvent;

        // ReSharper disable once NotNullMemberIsNotInitialized
        public GoalItemViewModel([NotNull] GoalModel goal) => GoalModel = goal;

        public void OnClick() => OnClickEvent?.Invoke(this);
    }
}