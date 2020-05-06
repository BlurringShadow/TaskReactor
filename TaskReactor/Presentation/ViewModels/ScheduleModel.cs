using System.ComponentModel.Composition;
using JetBrains.Annotations;
using Presentation.Utilities;

namespace Presentation.ViewModels
{
    public class ScheduleModel : ScreenViewModel
    {
        [ImportingConstructor]
        public ScheduleModel([NotNull] ArgsHelper argsArgsHelper) : base(argsArgsHelper)
        {
        }
    }
}