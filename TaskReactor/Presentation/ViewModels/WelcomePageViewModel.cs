using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class WelcomePageViewModel : ConductorOneActiveViewModel<ScreenViewModel>
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(MainScreenViewModel))] 
        public INavigationService NavigationService { get; set; }

        [ImportingConstructor, System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public WelcomePageViewModel([NotNull] CompositionContainer container) : base(container)
        {
            DisplayName = "Welcome";
            this.ShareWithName(NavigationService, nameof(NavigationService));
            Transition = false;
        }

        public bool Transition
        {
            get => ActiveItem!.InstanceType == typeof(RegisterViewModel);
            set => ActiveItem = value ?
                Container.GetExportedValue<RegisterViewModel>() :
                (ScreenViewModel)Container.GetExportedValue<LogInViewModel>();
        }
    }
}