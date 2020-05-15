using System.ComponentModel.Composition;
using ApplicationDomain.Models.Database.Entity;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "UnassignedReadonlyField")]
    public sealed class ScheduleEditViewModel : ScreenViewModel
    {
        [Args(typeof(WelcomePageViewModel))] public readonly User CurrentUser;

        [ImportingConstructor]
        public ScheduleEditViewModel([NotNull] ArgsHelper argsArgsHelper) : base(argsArgsHelper)
        {
        }
    }
}