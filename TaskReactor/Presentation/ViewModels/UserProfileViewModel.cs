using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class UserProfileViewModel : ScreenViewModel
    {
        [NotNull] private readonly IUserTaskService _userTaskService;
        [NotNull] private readonly IUserService _userService;

        [NotNull, ShareVariable(nameof(NavigationService), typeof(WelcomePageViewModel))]
        public INavigationService NavigationService { get; set; }

        [NotNull] private UserModel _currentUser;

        [NotNull, ShareVariable(nameof(CurrentUser), typeof(WelcomePageViewModel))]
        public UserModel CurrentUser { get => _currentUser; set => Set(ref _currentUser, value); }

        [NotNull] public string UserName => CurrentUser.Name;

        /// <summary>
        /// User Task collection
        /// </summary>
        [NotNull, ItemNotNull] public BindableCollection<UserTaskItemViewModel> UserTaskItems { get; } =
            new BindableCollection<UserTaskItemViewModel>();

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public UserProfileViewModel(
            [NotNull] CompositionContainer container,
            [NotNull] IUserTaskService userTaskService,
            [NotNull] IUserService userService
        ) : base(container)
        {
            _userService = userService;
            _userTaskService = userTaskService;
        }

        protected override Task OnActivateAsync(CancellationToken token)
        {
            // Refresh the user data
            var user = _userService.FindByKeysAsync(new[] { CurrentUser.Identity }, token).Result;

            if (user is null)
            {
                MessageBox.Show("用户不存在，请重新登录");
                NavigationService.NavigateToViewModel<WelcomePageViewModel>();
            }
            else
            {
                CurrentUser = user;
                RefreshTaskData(token);
            }

            return base.OnActivateAsync(token);
        }

        void RefreshTaskData(CancellationToken token)
        {
            var newTaskItemList = _userTaskService.GetAllFromUserAsync(CurrentUser, token).Result;

            {
                var i = 0;
                for (; i < newTaskItemList.Count && UserTaskItems.Count > i + 1; ++i)
                    UserTaskItems[i].Model = newTaskItemList[i]!;

                for (; i < newTaskItemList.Count; ++i)
                {
                    var itemViewModel = new UserTaskItemViewModel(newTaskItemList[i]!);
                    itemViewModel.OnClickEvent += ToUserTaskEdit;
                    itemViewModel.OnRemoveEvent += OnRemoveTask;
                    UserTaskItems.Add(itemViewModel);
                }
            }

            while (UserTaskItems.Count > newTaskItemList.Count) UserTaskItems.RemoveAt(UserTaskItems.Count - 1);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken token)
        {
            // Release the events
            foreach (var itemViewModel in UserTaskItems)
            {
                itemViewModel.OnClickEvent -= ToUserTaskEdit;
                itemViewModel.OnRemoveEvent -= OnRemoveTask;
            }

            return base.OnDeactivateAsync(close, token);
        }

        void NavigateToTaskEdit([NotNull] UserTaskModel newUserTaskModel)
        {
            this.ShareWithName(newUserTaskModel, nameof(UserTaskEditViewModel.TaskModel));
            this.ShareWithName(NavigationService, nameof(NavigationService));
            NavigationService.NavigateToViewModel<UserTaskEditViewModel>();
        }

        public void AddTask() => NavigateToTaskEdit(new UserTaskModel { OwnerUser = CurrentUser });

        void OnRemoveTask([NotNull] UserTaskItemViewModel viewModel)
        {
            viewModel.OnClickEvent -= ToUserTaskEdit;
            viewModel.OnRemoveEvent -= OnRemoveTask;

            UserTaskItems.Remove(viewModel);
            _userTaskService.Remove(viewModel.Model);
        }

        void ToUserTaskEdit([NotNull] UserTaskItemViewModel viewModel) => NavigateToTaskEdit(viewModel.Model);
    }
}