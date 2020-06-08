using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public class LogInViewModel : ScreenViewModel
    {
        [NotNull] private readonly INavigationService _navigationService;

        [NotNull] private readonly IUserService _userService;

        public string Password { get; set; }

        public string Identity { get; set; }

        [ImportingConstructor]
        public LogInViewModel(
            [NotNull] CompositionContainer container,
            [NotNull] IUserService userService,
            [NotNull, ShareVariable(nameof(_navigationService), typeof(WelcomePageViewModel))]
            INavigationService navigationService
        ) : base(container)
        {
            _userService = userService;
            _navigationService = navigationService;
        }

        public bool CanLogin => int.TryParse(Identity, out _) && !string.IsNullOrEmpty(Password);

        // TODO display login progress bar
        public void LogIn() =>
            Task.Run(
                async () =>
                {
                    var userModel = await _userService.LogInAsync(int.Parse(Identity!), Password!);

                    if(userModel is null) return;

                    this.ShareWithName(userModel, nameof(UserProfileViewModel.CurrentUser));
                    _navigationService.NavigateToViewModel<UserProfileViewModel>();
                }
            );
    }
}