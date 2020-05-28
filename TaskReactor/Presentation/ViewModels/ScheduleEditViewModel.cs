using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using ApplicationDomain.Models.Database.Entity;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class ScheduleEditViewModel : ScreenViewModel
    {
        public readonly User CurrentUser;

        [ImportingConstructor]
        public ScheduleEditViewModel(
            [NotNull] CompositionContainer container,
            [NotNull] IDictionary<(Type, string), ComposablePart> variableParts,
            [NotNull, Import(nameof(CurrentUser) + ":" + nameof(WelcomePageViewModel))]
            User currentUser
        ) : base(container, variableParts)
        {
            CurrentUser = currentUser;
        }
    }
}