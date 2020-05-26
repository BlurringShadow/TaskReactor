using System.ComponentModel.Composition;
using System.Windows.Controls;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export, System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class MainScreenViewModel : ScreenViewModel
    {
        [NotNull] public INavigationService NavigationService { get; private set; }

        [ImportingConstructor]
        public MainScreenViewModel([NotNull] ArgsHelper argsArgsHelper) : base(argsArgsHelper) => DisplayName = "Task Reactor";

        public void RegisterFrame([NotNull] Frame frame) => NavigationService = new FrameAdapter(frame);

        public void Navigate() => NavigationService.NavigateToViewModel<WelcomePageViewModel>();
    }
}