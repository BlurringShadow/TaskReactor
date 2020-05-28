#pragma warning disable 649

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
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
            [NotNull, ShareVariable(nameof(_navigationService) + ":" + nameof(MainScreenViewModel))]
            INavigationService navigationService,
            [NotNull] IDictionary<(Type, string), ComposablePart> variableParts
        ) : base(container, variableParts)
        {
            DisplayName = "Welcome";
            _navigationService = navigationService;
        }

        public void Login() => _navigationService.NavigateToViewModel<UserProfileViewModel>();
    }
}