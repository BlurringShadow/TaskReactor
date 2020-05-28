#pragma warning disable 649

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using ApplicationDomain.Models.Database.Entity;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized"),
     System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "UnassignedReadonlyField")]
    public sealed class UserProfileViewModel : ScreenViewModel
    {
        [NotNull] private readonly INavigationService _navigationService;

        [NotNull] public readonly User CurrentUser;

        [ImportingConstructor]
        public UserProfileViewModel([NotNull] CompositionContainer container,
            [NotNull, Import(nameof(CurrentUser) + ":" + nameof(WelcomePageViewModel))]
            User currentUser,
            [NotNull, Import(nameof(_navigationService) + ":" + nameof(WelcomePageViewModel))]
            INavigationService navigationService,
            [NotNull] IDictionary<(Type, string), ComposablePart> variableParts
        ) : base(container, variableParts)
        {
            CurrentUser = currentUser;
            _navigationService = navigationService;

            this.ForVariable(_navigationService).WithName(nameof(_navigationService)).Share();
        }

        public void AddSchedule() => _navigationService.NavigateToViewModel<ScheduleEditViewModel>();
    }
}