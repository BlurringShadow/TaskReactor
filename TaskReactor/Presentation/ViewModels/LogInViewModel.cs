using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;
using Presentation.ViewModels.UserProfile;

namespace Presentation.ViewModels
{
    [Export, System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class LogInViewModel : ScreenViewModel
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(WelcomePageViewModel))]
        public INavigationService NavigationService { get; set; }

        [NotNull] private readonly IUserService _userService;

        public string Password { get; set; }

        public string Identity { get; set; }

        [ImportingConstructor]
        public LogInViewModel([NotNull] CompositionContainer container, [NotNull] IUserService userService) :
            base(container) => _userService = userService;

        public bool CanLogin => int.TryParse(Identity, out _) && !string.IsNullOrEmpty(Password);

        // TODO display login progress bar
        public void LogIn() =>
            Task.Run(
                async () =>
                {
                    var userModel = await _userService.LogInAsync(int.Parse(Identity!), Password!);

                    if (userModel is null) return;

                    this.ShareWithName(userModel, nameof(UserProfileViewModel.CurrentUser));
                    NavigationService.NavigateToViewModel<UserProfileViewModel>();
                }
            );
    }
}