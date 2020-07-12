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
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class UserProfileViewModel : ConductorOneActiveViewModel<ScreenViewModel>
    {
        [NotNull] UserModel _currentUser;

        [NotNull, ShareVariable(nameof(CurrentUser), typeof(LogInViewModel))]
        public UserModel CurrentUser
        {
            get => _currentUser;
            set
            {
                Set(ref _currentUser, value);

                this.ShareWithName(CurrentUser, nameof(UserOverviewViewModel.CurrentUser));

                // ReSharper disable once PossibleNullReferenceException
                Items[0] = Container.GetExportedValue<UserOverviewViewModel>();
                Items[1] = Container.GetExportedValue<GraphEditPageViewModel>();
            }
        }

        [NotNull, Import] IUserService UserService { get; set; }

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

        protected override async Task ChangeActiveItemAsync(
            ScreenViewModel newItem,
            bool closePrevious,
            CancellationToken cancellationToken
        )
        {
            var task = base.ChangeActiveItemAsync(newItem, closePrevious, cancellationToken);
            if (!(task is null)) await task;
            NotifyOfPropertyChange(nameof(ActiveIndex));
        }

        // ReSharper disable once NotNullMemberIsNotInitialized
        [ImportingConstructor]
        public UserProfileViewModel([NotNull] IocContainer container) :
            base(container)
        {
            this.ShareWithName(NavigationService, nameof(NavigationService));

            // ReSharper disable once PossibleNullReferenceException
            Items.Add(null);
            Items.Add(null);
        }

        protected override async Task OnActivateAsync(CancellationToken token)
        {
            var task = base.OnActivateAsync(token)!;
            // Refresh the user data
            if (await UserService.FindByKeysAsync(token, CurrentUser.Identity) is null)
            {
                MessageBox.Show("用户不存在，请重新登录");
                NavigationService.NavigateToViewModel<WelcomePageViewModel>();
            }

            if (!(task is null)) await task;
        }
    }
}