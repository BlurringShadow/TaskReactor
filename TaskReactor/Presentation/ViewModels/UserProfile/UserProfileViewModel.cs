using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;
using Presentation.ViewModels.UserProfile.Overview;
using Presentation.ViewModels.UserProfile.TaskDependencyGraph;
using Presentation.ViewModels.WelcomePage;
using Utilities;

namespace Presentation.ViewModels.UserProfile
{
    [Export]
    public sealed class UserProfileViewModel : ConductorOneActiveViewModel<ScreenViewModel>
    {
        [NotNull] UserModel _currentUser;

        [NotNull, ShareVariable(nameof(CurrentUser), typeof(LogInViewModel))]
        public UserModel CurrentUser { get => _currentUser; set => Set(ref _currentUser, value); }

        [NotNull] readonly IUserService _userService;

        [NotNull, ShareVariable(nameof(NavigationService), typeof(LogInViewModel))]
        public INavigationService NavigationService { get; set; }

        public int ActiveIndex
        {
            // ReSharper disable once PossibleNullReferenceException
            get => Items.IndexOf(ActiveItem);
            set
            {
                switch (value)
                {
                    case 2:
                        NavigationService.NavigateToViewModel<WelcomePageViewModel>();
                        return;

                    case 3:
                        MessageBox.Show("设置页面待完成");
                        return;

                    default:
                        ActiveItem = Items?[value];
                        return;
                }
            }
        }

        protected override Task ChangeActiveItemAsync(
            ScreenViewModel newItem,
            bool closePrevious,
            CancellationToken cancellationToken
        )
        {
            NotifyOfPropertyChange(nameof(ActiveIndex));
            return base.ChangeActiveItemAsync(newItem, closePrevious, cancellationToken);
        }

        // ReSharper disable once NotNullMemberIsNotInitialized
        public UserProfileViewModel([NotNull] IocContainer container, [NotNull] IUserService userService) :
            base(container)
        {
            _userService = userService;

            // ReSharper disable once PossibleNullReferenceException
            Items.Add(Container.GetExportedValue<UserOverviewViewModel>());
            Items.Add(Container.GetExportedValue<GraphEditPageViewModel>());
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
                this.ShareWithName(CurrentUser, nameof(UserOverviewViewModel.CurrentUser));
                // ReSharper disable once PossibleNullReferenceException
                Items[0] = Container.GetExportedValue<UserOverviewViewModel>();
                Items[1] = Container.GetExportedValue<GraphEditPageViewModel>();
            }

            await base.OnActivateAsync(token)!;
        }
    }
}