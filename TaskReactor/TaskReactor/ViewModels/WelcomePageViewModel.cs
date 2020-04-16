using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace TaskReactor.ViewModels
{
    [Export]
    public class WelcomePageViewModel : Screen
    {
        public WelcomePageViewModel() => DisplayName = "Welcome";
    }
}