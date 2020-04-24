using System.ComponentModel.Composition;
using JetBrains.Annotations;
using TaskReactor.Utilities;

namespace TaskReactor.ViewModels
{
    [Export]
    public class WelcomePageViewModel : ScreenViewModel
    {
        [ImportingConstructor]
        public WelcomePageViewModel([NotNull] ArgsHelper argsArgsHelper) : base(argsArgsHelper) =>
            DisplayName = "Welcome";
    }
}