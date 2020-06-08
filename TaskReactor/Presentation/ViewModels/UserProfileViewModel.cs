using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using ApplicationDomain.DataModel;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class UserProfileViewModel : ScreenViewModel
    {
        [NotNull] private readonly INavigationService _navigationService;

        [NotNull] public readonly UserModel CurrentUser;

        [ImportingConstructor]
        public UserProfileViewModel(
            [NotNull] CompositionContainer container,
            [NotNull, ShareVariable(nameof(CurrentUser), typeof(WelcomePageViewModel))]
            UserModel currentUser,
            [NotNull, ShareVariable(nameof(_navigationService), typeof(WelcomePageViewModel))]
            INavigationService navigationService
        ) : base(container)
        {
            CurrentUser = currentUser;
            _navigationService = navigationService;
        }

        /// <summary>
        /// TODO: add task list to select
        /// </summary>
        public void AddTask()
        {
            this.ShareWithName(_navigationService, nameof(_navigationService));
            _navigationService.NavigateToViewModel<GoalEditViewModel>();
        }
    }
}