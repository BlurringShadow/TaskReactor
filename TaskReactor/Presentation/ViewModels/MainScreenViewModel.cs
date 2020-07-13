using System.ComponentModel.Composition;
using Caliburn.Micro;
using JetBrains.Annotations;
using Presentation.ViewModels.WelcomePage;
using Presentation.Views;
using Utilities;

namespace Presentation.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class MainScreenViewModel : ScreenViewModel
    {
        [NotNull] private INavigationService _navigationService;

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public MainScreenViewModel([NotNull] IocContainer container) : base(container) =>
            DisplayName = "Task Reactor";

        /// <summary> Initialize the window navigation service </summary>
        public void RegisterWindow([NotNull] MainScreenView window)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            _navigationService = new CMNavigationService(window.NavigationService);
            Navigate();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void Navigate() => _navigationService.For<WelcomePageViewModel>().WithParam(vm => vm.NavigationService, _navigationService).Navigate();
    }
}