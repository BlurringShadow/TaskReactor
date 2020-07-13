#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 07
// Day: 08
// Time: 下午 10:27

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels.UserProfile.Overview
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class UserOverviewViewModel : ScreenViewModel, IAsyncDisposable, IDisposable
    {
        [NotNull, Import] private IUserTaskService UserTaskService { get; set; }

        [NotNull] public INavigationService NavigationService { get; set; }

        [NotNull] UserModel _currentUser;

        [NotNull] public UserModel CurrentUser
        {
            get => _currentUser;
            set
            {
                Set(ref _currentUser, value);
                _ = RefreshTaskData();
            }
        }

        [NotNull] CancellationTokenSource _refreshDataTokenSource = new CancellationTokenSource();

        /// <summary>
        /// User Task collection
        /// </summary>
        [NotNull, ItemNotNull] public BindableCollection<UserTaskItemViewModel> UserTaskItems { get; } =
            new BindableCollection<UserTaskItemViewModel>();

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public UserOverviewViewModel([NotNull] IocContainer container) : base(container)
        {
        }

        protected override Task OnActivateAsync(CancellationToken token)
        {
            Task.Run(RefreshTaskData, token);
            // ReSharper disable once PossibleNullReferenceException
            return base.OnActivateAsync(_refreshDataTokenSource.Token);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            lock (_refreshDataTokenSource)
                if (!_refreshDataTokenSource.IsCancellationRequested)
                    _refreshDataTokenSource.Cancel();
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        async Task RefreshTaskData()
        {
            lock (_refreshDataTokenSource)
            {
                if (!_refreshDataTokenSource.IsCancellationRequested)
                {
                    _refreshDataTokenSource.Cancel();
                    _refreshDataTokenSource.Dispose();
                }

                _refreshDataTokenSource = new CancellationTokenSource();
            }

            var token = _refreshDataTokenSource.Token;

            IList<UserTaskModel> newTaskItemList = null;

            await Task.Run(
                () =>
                {
                    lock (UserTaskService)
                        newTaskItemList = UserTaskService.GetAllFromUserAsync(CurrentUser, token).Result;
                }, token
            );

            var i = 0;
            var newCount = newTaskItemList.Count;
            lock (UserTaskItems)
            {
                var oldCount = UserTaskItems.Count;

                if (newCount > oldCount)
                {
                    for (; i < oldCount; ++i)
                        UserTaskItems[i].TaskModel = newTaskItemList[i]!;

                    for (; i < newCount; ++i)
                    {
                        var itemViewModel = new UserTaskItemViewModel(
                            newTaskItemList[i]!, Container.GetExportedValue<IGoalService>()
                        );
                        itemViewModel.OnClickEvent += ToUserTaskEdit;
                        itemViewModel.OnRemoveEvent += OnRemoveTask;
                        itemViewModel.OnGoalClickEvent += ToGoalEdit;
                        itemViewModel.OnAddGoalEvent += AddGoal;
                        UserTaskItems.Add(itemViewModel);
                    }
                }
                else
                {
                    for (; i < newCount; ++i)
                        UserTaskItems[i].TaskModel = newTaskItemList[i]!;

                    while (UserTaskItems.Count > newTaskItemList.Count) UserTaskItems.RemoveAt(UserTaskItems.Count - 1);
                }
            }
        }

        void NavigateToTaskEdit([NotNull] UserTaskModel taskModel)
        {
            this.DeactivateAsync(false);

            this.ShareWithName(taskModel, nameof(UserTaskEditViewModel.TaskModel));
            this.ShareWithName(NavigationService, nameof(NavigationService));
            NavigationService.NavigateToViewModel<UserTaskEditViewModel>();
        }

        public void AddTask() => NavigateToTaskEdit(new UserTaskModel { OwnerUser = CurrentUser });

        void OnRemoveTask([NotNull] UserTaskItemViewModel viewModel)
        {
            viewModel.OnClickEvent -= ToUserTaskEdit;
            viewModel.OnRemoveEvent -= OnRemoveTask;

            UserTaskItems.Remove(viewModel);
            UserTaskService.Remove(viewModel.TaskModel);
        }

        void ToUserTaskEdit([NotNull] UserTaskItemViewModel viewModel) => NavigateToTaskEdit(viewModel.TaskModel);

        void NavigateToGoalEdit([NotNull] GoalModel goalModel)
        {
            this.DeactivateAsync(false);

            this.ShareWithName(goalModel, nameof(GoalEditViewModel.GoalModel));
            this.ShareWithName(NavigationService, nameof(GoalEditViewModel.NavigationService));

            NavigationService.NavigateToViewModel<GoalEditViewModel>();
        }

        void ToGoalEdit([NotNull] GoalItemViewModel viewModel) => NavigateToGoalEdit(viewModel.GoalModel);

        void AddGoal([NotNull] UserTaskItemViewModel viewModel) =>
            NavigateToGoalEdit(new GoalModel { FromTask = viewModel.TaskModel });

        public ValueTask DisposeAsync()
        {
            if (IsActive)
            {
                var task = this.DeactivateAsync(true);
                if (!(task is null))
                    return new ValueTask(task);
            }

            _refreshDataTokenSource.Cancel();
            _refreshDataTokenSource.Dispose();

            return default;
        }

        public void Dispose() => DisposeAsync().AsTask().Wait();
    }
}