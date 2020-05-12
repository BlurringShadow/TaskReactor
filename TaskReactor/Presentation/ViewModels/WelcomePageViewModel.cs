#pragma warning disable 649

using System.ComponentModel.Composition;
using Caliburn.Micro;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels
{
    [Export, System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class WelcomePageViewModel : ScreenViewModel
    {
        [Args(typeof(MainScreenViewModel)), NotNull]
        private readonly INavigationService _navigationService;

        private string _userName;

        public string UserName { get => _userName; set => Set(ref _userName, value); }

        private string _userPassword;

        public string UserPassword { get => _userPassword; set => Set(ref _userPassword, value); }

        [ImportingConstructor]
        public WelcomePageViewModel([NotNull] ArgsHelper argsArgsHelper) : base(argsArgsHelper) 
            => DisplayName = "Welcome";

        public void Login()
        {
            this.Update<UserProfileViewModel>(_navigationService, nameof(_navigationService));
        }
    }
}