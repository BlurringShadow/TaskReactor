using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels.WelcomePage
{
    [Export]
    public sealed class WelcomePageViewModel : ConductorOneActiveViewModel<ScreenViewModel>
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(MainScreenViewModel))]
        public INavigationService NavigationService { get; set; }

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public WelcomePageViewModel([NotNull] CompositionContainer container) : base(container)
        {
            DisplayName = "Welcome";

            this.ShareWithName(NavigationService, nameof(LogInViewModel.NavigationService));

            // ReSharper disable once PossibleNullReferenceException
            Items.Add(Container.GetExportedValue<LogInViewModel>());
            Items.Add(Container.GetExportedValue<RegisterViewModel>());
        }
    }
}