using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
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

        [NotNull, ShareVariable(nameof(CurrentUser), typeof(WelcomePageViewModel))]
        public UserModel CurrentUser { get; set; }

        [NotNull] public string UserName => CurrentUser.Name;

        /// <summary>
        /// User Task collection
        /// </summary>
        [NotNull, ItemNotNull] public BindableCollection<UserTaskItemViewModel> UserTaskItems { get; set; } =
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

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            var user = _userService.FindByKeysAsync(new[] { CurrentUser.Identity }, cancellationToken).Result;

            if (user is null)
            {
                MessageBox.Show("用户不存在，请重新登录");
                NavigationService.NavigateToViewModel<WelcomePageViewModel>();
            }
            else
            {
                CurrentUser = user;

                // Refresh the task data
                foreach (var userTaskItemViewModel in from userTaskModel in
                        _userTaskService.GetAllFromUserAsync(CurrentUser, cancellationToken).Result
                    select new UserTaskItemViewModel(userTaskModel))
                {
                    userTaskItemViewModel!.OnClickEvent += ToUserTaskEdit;
                    UserTaskItems.Add(userTaskItemViewModel);
                }
            }

            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            // Release the events
            foreach (var userTaskItemViewModel in UserTaskItems) userTaskItemViewModel.OnClickEvent -= ToUserTaskEdit;

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public void AddTask() => NavigateToTaskEdit(new UserTaskModel { OwnerUser = CurrentUser });

        void NavigateToTaskEdit([NotNull] UserTaskModel newUserTaskModel)
        {
            this.ShareWithName(newUserTaskModel, nameof(UserTaskEditViewModel.TaskModel));
            this.ShareWithName(NavigationService, nameof(NavigationService));
            NavigationService.NavigateToViewModel<UserTaskEditViewModel>();
        }

        void ToUserTaskEdit([NotNull] UserTaskModel model) => NavigateToTaskEdit(model);
    }
}