using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;
using Presentation.ViewModels.WelcomePage;
using Utilities;

namespace Presentation.ViewModels.UserProfile
{
    [Export]
    public sealed class UserProfileViewModel : ScreenViewModel
    {
        [NotNull] readonly IUserTaskService _userTaskService;

        [NotNull] readonly IUserService _userService;

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
                NotifyOfPropertyChange(nameof(UserName));
                _ = RefreshTaskData(CancellationToken.None);
            }
        }

        [NotNull] public string UserName => CurrentUser.Name;

        /// <summary>
        /// User Task collection
        /// </summary>
        [NotNull, ItemNotNull] public BindableCollection<UserTaskItemViewModel> UserTaskItems { get; } =
            new BindableCollection<UserTaskItemViewModel>();

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public UserProfileViewModel(
            [NotNull] IocContainer container,
            [NotNull] IUserTaskService userTaskService,
            [NotNull] IUserService userService
        ) : base(container)
        {
            _userService = userService;
            _userTaskService = userTaskService;
        }

        protected override async Task OnActivateAsync(CancellationToken token)
        {
            // Refresh the user data
            var user = await _userService.FindByKeysAsync(token, CurrentUser.Identity);

            if (user is null)
            {
                MessageBox.Show("用户不存在，请重新登录");
                NavigationService.NavigateToViewModel<WelcomePageViewModel>();
            }
            else
            {
                CurrentUser = user;
                _ = RefreshTaskData(token);
            }

            await base.OnActivateAsync(token)!;
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