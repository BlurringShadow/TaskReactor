using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Controls;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class MainScreenViewModel : ScreenViewModel
    {
        [NotNull] private INavigationService _navigationService;

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public MainScreenViewModel([NotNull] CompositionContainer container) : base(container) =>
            DisplayName = "Task Reactor";

        /// <summary> Initialize the frame navigation service </summary>
        public void RegisterFrame([NotNull] Frame frame)
        {
            _navigationService = new FrameAdapter(frame);
            this.ShareWithName(_navigationService, nameof(WelcomePageViewModel.NavigationService));
        }

        public void Navigate() => _navigationService.NavigateToViewModel<WelcomePageViewModel>();
    }
}