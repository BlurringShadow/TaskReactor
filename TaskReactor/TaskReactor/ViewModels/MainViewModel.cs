using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Controls;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace TaskReactor.ViewModels
{
    [Export]
    public class MainViewModel : Screen
    {
        public INavigationService NavigationService { get; private set; }

        public MainViewModel() => DisplayName = "Task Reactor";

        public void RegisterFrame([NotNull] Frame frame) => NavigationService = new FrameAdapter(frame);

        public void Navigate()
        {
            Debug.Assert(NavigationService != null, nameof(NavigationService) + " != null");
            NavigationService.NavigateToViewModel<WelcomePageViewModel>();
        }
    }
}