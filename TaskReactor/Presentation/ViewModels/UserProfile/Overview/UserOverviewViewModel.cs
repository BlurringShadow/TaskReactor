#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 07
// Day: 08
// Time: 下午 10:27

#endregion

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;
using Presentation.ViewModels.WelcomePage;
using Utilities;

namespace Presentation.ViewModels.UserProfile.Overview
{
    [Export]
    public class UserOverviewViewModel : ScreenViewModel
    {
        [NotNull] readonly IUserTaskService _userTaskService;

        [NotNull, ShareVariable(nameof(NavigationService), typeof(LogInViewModel))]
        public INavigationService NavigationService { get; set; }

        [NotNull] UserModel _currentUser;

        [NotNull, ShareVariable(nameof(CurrentUser), typeof(LogInViewModel))]
        public UserModel CurrentUser
        {
            get => _currentUser;
            set
            {
                Set(ref _currentUser, value);
                _ = RefreshTaskData(CancellationToken.None);
            }
        }

        /// <summary>
        /// User Task collection
        /// </summary>
        [NotNull, ItemNotNull] public BindableCollection<UserTaskItemViewModel> UserTaskItems { get; } =
            new BindableCollection<UserTaskItemViewModel>();

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public UserOverviewViewModel(
            [NotNull] IocContainer container,
            [NotNull] IUserTaskService userTaskService
        ) : base(container) => _userTaskService = userTaskService;

        protected override Task OnActivateAsync(CancellationToken token)
        {
            _ = RefreshTaskData(token);
            return Task.CompletedTask;
        }

        async Task RefreshTaskData(CancellationToken token)
        {
            var newTaskItemList = await _userTaskService.GetAllFromUserAsync(CurrentUser, token);

            var i = 0;
            var newCount = newTaskItemList.Count;
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
            _userTaskService.Remove(viewModel.TaskModel);
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
    }
}