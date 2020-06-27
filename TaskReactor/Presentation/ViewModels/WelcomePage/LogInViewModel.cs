using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using System.Windows.Controls;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;
using Presentation.ViewModels.UserProfile;

namespace Presentation.ViewModels.WelcomePage
{
    [Export]
    public class LogInViewModel : ScreenViewModel
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(WelcomePageViewModel))]
        public INavigationService NavigationService { get; set; }

        [NotNull] private readonly IUserService _userService;

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                Set(ref _password, value);
                NotifyOfPropertyChange(nameof(CanLogin));
            }
        }

        public string Identity { get; private set; }

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public LogInViewModel([NotNull] CompositionContainer container, [NotNull] IUserService userService) :
            base(container) => _userService = userService;

        public bool CanLogin => int.TryParse(Identity, out _) && !string.IsNullOrEmpty(Password);

        public void SetPassword([NotNull] PasswordBox value) => Password = value.Password;

        // TODO display login progress bar
        public async void LogIn()
        {
            var userModel = await _userService.LogInAsync(int.Parse(Identity!), Password!);
            if (userModel is null)
            {
                MessageBox.Show("incorrect password or id not exists");
                return;
            }

            this.ShareWithName(userModel, nameof(UserProfileViewModel.CurrentUser));
            NavigationService.NavigateToViewModel<UserProfileViewModel>();
        }
    }
}