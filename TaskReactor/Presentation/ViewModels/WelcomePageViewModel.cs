#pragma warning disable 649

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export, System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class WelcomePageViewModel : ScreenViewModel
    {
        [NotNull] private readonly INavigationService _navigationService;

        private string _userName;

        public string UserName { get => _userName; set => Set(ref _userName, value); }

        private string _userPassword;

        public string UserPassword { get => _userPassword; set => Set(ref _userPassword, value); }

        [ImportingConstructor]
        public WelcomePageViewModel(
            [NotNull] CompositionContainer container,
            [NotNull, Import(nameof(_navigationService) + ":" + nameof(MainScreenViewModel))]
            INavigationService navigationService) : base(container)
        {
            DisplayName = "Welcome";
            _navigationService = navigationService;
            this.ShareForWithName(_navigationService, nameof(_navigationService));
        }

        public void Login() => _navigationService.NavigateToViewModel<UserProfileViewModel>();
    }
}