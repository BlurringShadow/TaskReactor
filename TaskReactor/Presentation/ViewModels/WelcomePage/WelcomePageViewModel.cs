using System.ComponentModel.Composition;
using Caliburn.Micro;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels.WelcomePage
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class WelcomePageViewModel : ConductorOneActiveViewModel<ScreenViewModel>
    {
        [NotNull] private INavigationService _navigationService;

        [NotNull, System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public INavigationService NavigationService
        {
            get => _navigationService;
            set
            {
                ((LogInViewModel)Items[0]).NavigationService = value;
                _navigationService = value;
            }
        }

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public WelcomePageViewModel([NotNull] IocContainer container) : base(container)
        {
            DisplayName = "Welcome";

            this.ShareWithName(NavigationService, nameof(LogInViewModel.NavigationService));

            // ReSharper disable once PossibleNullReferenceException
            Items.Add(Container.GetExportedValue<LogInViewModel>());
            Items.Add(Container.GetExportedValue<RegisterViewModel>());
        }
    }
}