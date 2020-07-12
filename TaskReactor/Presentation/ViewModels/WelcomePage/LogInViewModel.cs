using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;
using Presentation.ViewModels.UserProfile;
using Utilities;

namespace Presentation.ViewModels.WelcomePage
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class LogInViewModel : ScreenViewModel
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(MainScreenViewModel))]
        public INavigationService NavigationService { get; set; }

        [NotNull, Import] public IUserService UserService { get; set; }

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

        private string _identity;

        public string Identity
        {
            get => _identity;
            set
            {
                Set(ref _identity, value);
                NotifyOfPropertyChange(nameof(CanLogin));
            }
        }

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public LogInViewModel([NotNull] IocContainer container) : base(container)
        {
        }

        public bool CanLogin => int.TryParse(Identity, out _) && !string.IsNullOrEmpty(Password);

        public void SetPassword([NotNull] PasswordBox value) => Password = value.Password;

        // TODO display login progress bar
        public async void LogIn()
        {
            try
            {
                var userModel = await UserService.LogInAsync(int.Parse(Identity!), Password!);
                if (userModel is null)
                {
                    MessageBox.Show("incorrect password or id not exists");
                    return;
                }

                this.ShareWithName(userModel, nameof(UserProfileViewModel.CurrentUser));
                this.ShareWithName(NavigationService, nameof(UserProfileViewModel.NavigationService));
                NavigationService.NavigateToViewModel<UserProfileViewModel>();
            }
            catch (Exception e) { MessageBox.Show($"{e}"); }
        }
    }
}