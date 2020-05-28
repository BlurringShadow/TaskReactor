using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using ApplicationDomain.Models.Database.Entity;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "UnassignedReadonlyField")]
    public sealed class ScheduleEditViewModel : ScreenViewModel
    {
        public readonly User CurrentUser;

        [ImportingConstructor]
        public ScheduleEditViewModel([NotNull] CompositionContainer container) : base(container)
        {
        }
    }
}