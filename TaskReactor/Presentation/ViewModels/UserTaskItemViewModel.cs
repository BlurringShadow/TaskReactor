#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 09
// Time: 上午 11:49

#endregion

using System;
using System.Globalization;
using System.Threading;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    public sealed partial class UserProfileViewModel
    {
        public sealed partial class UserTaskItemViewModel : Screen
        {
            [NotNull] UserTaskModel _taskModel;

            [NotNull] readonly IGoalService _goalService;

            [NotNull] public UserTaskModel TaskModel
            {
                get => _taskModel;
                set
                {
                    Set(ref _taskModel, value);
                    RefreshGoalsData(CancellationToken.None);
                    NotifyOfPropertyChange(nameof(DisplayName));
                    NotifyOfPropertyChange(nameof(TaskStartTime));
                }
            }

            [NotNull] public override string DisplayName
            {
                get => TaskModel.Title;
                set
                {
                    TaskModel.Title = value;
                    base.DisplayName = value;
                }
            }

            [NotNull] public string TaskStartTime => TaskModel.StartTime.ToString(CultureInfo.CurrentCulture);

            public event Action<UserTaskItemViewModel> OnClickEvent;

            public event Action<UserTaskItemViewModel> OnRemoveEvent;

            public event Action<GoalItemViewModel> OnGoalClickEvent;

            /// <summary>
            /// User Task collection
            /// </summary>
            [NotNull, ItemNotNull] public BindableCollection<GoalItemViewModel> GoalItems { get; } =
                new BindableCollection<GoalItemViewModel>();

            // ReSharper disable once NotNullMemberIsNotInitialized
            public UserTaskItemViewModel([NotNull] UserTaskModel taskModel, [NotNull] IGoalService goalService)
            {
                TaskModel = taskModel;
                _goalService = goalService;
            }

            void RefreshGoalsData(CancellationToken token)
            {
                var goalList = _goalService.GetAllFromTaskAsync(TaskModel, token).Result;

                var i = 0;
                var newCount = goalList.Count;
                var oldCount = GoalItems.Count;

                if (newCount > oldCount)
                {
                    for (; i < oldCount; ++i)
                        GoalItems[i].GoalModel = goalList[i]!;

                    for (; i < newCount; ++i)
                    {
                        var itemViewModel = new GoalItemViewModel(goalList[i]!);
                        itemViewModel.OnClickEvent += OnGoalClick;
                        GoalItems.Add(itemViewModel);
                    }
                }
                else
                {
                    for (; i < newCount; ++i)
                        GoalItems[i].GoalModel = goalList[i]!;

                    while (GoalItems.Count > goalList.Count) GoalItems.RemoveAt(GoalItems.Count - 1);
                }
            }

            void OnGoalClick(GoalItemViewModel viewModel) => OnGoalClickEvent?.Invoke(viewModel);

            public void OnClick() => OnClickEvent?.Invoke(this);

            public void OnRemove() => OnRemoveEvent?.Invoke(this);
        }
    }
}