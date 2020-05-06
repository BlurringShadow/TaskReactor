using System.ComponentModel.Composition;
using JetBrains.Annotations;
using Presentation.Utilities;

namespace Presentation.ViewModels
{
    [Export]
    public class WelcomePageViewModel : ScreenViewModel
    {
        [ImportingConstructor]
        public WelcomePageViewModel([NotNull] ArgsHelper argsArgsHelper) : base(argsArgsHelper) =>
            DisplayName = "Welcome";
    }
}