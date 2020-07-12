using System.ComponentModel.Composition;
using Caliburn.Micro;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels.WelcomePage
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class WelcomePageViewModel : ConductorOneActiveViewModel<ScreenViewModel>
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(MainScreenViewModel))]
        public INavigationService NavigationService { get; set; }

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