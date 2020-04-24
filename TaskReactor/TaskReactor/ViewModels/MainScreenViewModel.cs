using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Controls;
using Caliburn.Micro;
using JetBrains.Annotations;
using TaskReactor.Utilities;

namespace TaskReactor.ViewModels
{
    [Export]
    public class MainScreenViewModel : ScreenViewModel
    {
        public INavigationService NavigationService { get; private set; }

        [ImportingConstructor]
        public MainScreenViewModel([NotNull] ArgsHelper argsArgsHelper) : base(argsArgsHelper) => DisplayName = "Task Reactor";

        public void RegisterFrame([NotNull] Frame frame) => NavigationService = new FrameAdapter(frame);

        public void Navigate()
        {
            Debug.Assert(NavigationService != null, nameof(NavigationService) + " != null");

            NavigationService.For<WelcomePageViewModel>()
                ?
                .Navigate();
        }
    }
}